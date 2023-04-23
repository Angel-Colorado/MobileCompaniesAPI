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
    public class PositionsController : ControllerBase
    {
        private readonly MobileCoContext _context;

        public PositionsController(MobileCoContext context)
        {
            _context = context;
        }

        // GET: api/Positions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PositionDTO>>> GetPositions()
        {
            var positionDTOs = await _context.Positions
                .Select(p => new PositionDTO
                {
                    ID = p.ID,
                    TimeStamp = p.TimeStamp,
                    Latitude = p.Latitude,
                    Longitude = p.Longitude,
                    DeviceID = p.DeviceID,
                })
                .OrderByDescending(p => p.TimeStamp)
                .ToListAsync();

            if (positionDTOs.Count() > 0)
            {
                return positionDTOs;
            }
            else
            {
                return NotFound(new { message = "Error: No Position records" });
            }
        }

        // PUT: api/Positions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPosition(int id, PositionDTO positionDTO)
        {
            if (id != positionDTO.ID)
            {
                return BadRequest(new { message = "Error: ID does not match Position" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Get the record you want to update
            var positionToUpdate = await _context.Positions.FindAsync(id);

            // Check that you got it
            if (positionToUpdate == null)
            {
                return NotFound(new { message = "Error: Position record not found" });
            }

            // Update the properties of the entity object from the DTO object
            positionToUpdate.ID = positionDTO.ID;
            positionToUpdate.TimeStamp = positionDTO.TimeStamp;
            positionToUpdate.Latitude = positionDTO.Latitude;
            positionToUpdate.Longitude = positionDTO.Longitude;
            positionToUpdate.DeviceID = positionDTO.DeviceID;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PositionExists(id))
                {
                    return Conflict(new { message = "Concurrency Error: Position has been Removed" });
                }
                else
                {
                    return Conflict(new { message = "Concurrency Error: Position has been updated by another user. Back out and try editing the record again" });
                }
            }
            catch (DbUpdateException)
            {
                return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persists see your system administrator" });
            }
        }

        // POST: api/Positions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PositionDTO>> PostPosition(PositionDTO positionDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Position position = new Position
            {
                ID = positionDTO.ID,
                TimeStamp = positionDTO.TimeStamp,
                Latitude = positionDTO.Latitude,
                Longitude = positionDTO.Longitude,
                DeviceID = positionDTO.DeviceID
            };

            try
            {
                _context.Positions.Add(position);
                await _context.SaveChangesAsync();

                // Assign Database Generated values back into the DTO
                positionDTO.ID = position.ID;

                return NoContent();
            }
            catch (DbUpdateException)
            {
                return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persists see your system administrator" });
            }
        }

        // DELETE: api/Positions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePosition(int id)
        {
            var position = await _context.Positions.FindAsync(id);

            if (position == null)
            {
                return NotFound(new { message = "Delete Error: Position has already been removed" });
            }

            try
            {
                _context.Positions.Remove(position);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException)
            {
                return BadRequest(new { message = "Delete Error: Unable to delete Position" });
            }
        }

        private bool PositionExists(int id)
        {
            return _context.Positions.Any(e => e.ID == id);
        }
    }
}
