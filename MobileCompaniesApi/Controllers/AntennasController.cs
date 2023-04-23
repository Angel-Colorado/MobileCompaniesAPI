using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobileCompaniesApi.Data;
using MobileCompaniesApi.Models;

namespace MobileCompaniesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AntennasController : ControllerBase
    {
        private readonly MobileCoContext _context;

        public AntennasController(MobileCoContext context)
        {
            _context = context;
        }

        // GET: api/Antennas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AntennaDTO>>> GetAntennas()
        {
            var antennaDTOs = await _context.Antennas
                .Include(c => c.Company)
                .Select(a => new AntennaDTO
                {
                    ID = a.ID,
                    Name = a.Name,
                    Latitude = a.Latitude,
                    Longitude = a.Longitude,
                    Description = a.Description,
                    Image = a.Image,
                    CompanyID = a.CompanyID,
                    Company = new CompanyDTO
                    {
                        ID = a.Company.ID,
                        Name = a.Company.Name
                    }
                })
                .ToListAsync();

            if (antennaDTOs.Count() > 0)
            {
                return antennaDTOs;
            }
            else
            {
                return NotFound(new { message = "Error: No Antenna records" });
            }
        }

        // GET: api/Antennas/ByCompany
        [HttpGet("ByCompany/{id}")]
        public async Task<ActionResult<IEnumerable<AntennaDTO>>> GetAntennasByCompany(int id)
        {
            var antennaDTOs = await _context.Antennas
                .Include(c => c.Company)
                .Select(a => new AntennaDTO
                {
                    ID = a.ID,
                    Name = a.Name,
                    Latitude = a.Latitude,
                    Longitude = a.Longitude,
                    Description = a.Description,
                    Image = a.Image,
                    CompanyID = a.CompanyID,
                    Company = new CompanyDTO
                    {
                        ID = a.Company.ID,
                        Name = a.Company.Name
                    }
                })
                .Where(c => c.CompanyID == id)
                .ToListAsync();

            if (antennaDTOs.Count() > 0)
            {
                return antennaDTOs;
            }
            else
            {
                return NotFound(new { message = "Error: No Antenna records for that Company" });
            }
        }

        // PUT: api/Antennas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAntenna(int id, AntennaDTO antennaDTO)
        {
            if (id != antennaDTO.ID)
            {
                return BadRequest(new { message = "Error: ID does not match Antenna" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Get the record you want to update
            var antennaToUpdate = await _context.Antennas.FindAsync(id);

            //Check that you got it
            if (antennaToUpdate == null)
            {
                return NotFound(new { message = "Error: Antenna record not found" });
            }

            // Update the properties of the entity object from the DTO object
            antennaToUpdate.ID = antennaDTO.ID;
            antennaToUpdate.Name = antennaDTO.Name;
            antennaToUpdate.Latitude = antennaDTO.Latitude;
            antennaToUpdate.Longitude = antennaDTO.Longitude;
            antennaToUpdate.Description = antennaDTO.Description;
            antennaToUpdate.Image = antennaDTO.Image;
            antennaToUpdate.CompanyID = antennaDTO.CompanyID;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AntennaExists(id))
                {
                    return Conflict(new { message = "Concurrency Error: Antenna has been Removed" });
                }
                else
                {
                    return Conflict(new { message = "Concurrency Error: Antenna has been updated by another user. Back out and try editing the record again" });
                }
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("UNIQUE"))
                {
                    return BadRequest(new { message = "Unable to save: Duplicate Location (Latitude-Longitude pair)" });
                }
                else
                {
                    return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persists see your system administrator" });
                }
            }
        }

        // POST: api/Antennas
        [HttpPost]
        public async Task<ActionResult<Antenna>> PostAntenna(AntennaDTO antennaDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Antenna antenna = new Antenna
            {
                ID = antennaDTO.ID,
                Name = antennaDTO.Name,
                Latitude = antennaDTO.Latitude,
                Longitude = antennaDTO.Longitude,
                Description = antennaDTO.Description,
                Image = antennaDTO.Image,
                CompanyID = antennaDTO.CompanyID
            };

            try
            {
                _context.Antennas.Add(antenna);
                await _context.SaveChangesAsync();

                // Assign Database Generated values back into the DTO
                antennaDTO.ID = antenna.ID;

                return NoContent();
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("UNIQUE"))
                {
                    return BadRequest(new { message = "Unable to save: Duplicate Location (Latitude-Longitude pair)" });
                }
                else
                {
                    return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persists see your system administrator" });
                }
            }
        }

        // DELETE: api/Antennas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAntenna(int id)
        {
            var antenna = await _context.Antennas.FindAsync(id);

            if (antenna == null)
            {
                return NotFound(new { message = "Delete Error: Antenna has already been removed" });
            }

            try
            {
                _context.Antennas.Remove(antenna);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException)
            {
                return BadRequest(new { message = "Delete Error: Unable to delete Antenna" });
            }
        }

        private bool AntennaExists(int id)
        {
            return _context.Antennas.Any(e => e.ID == id);
        }
    }
}
