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
    public class CompaniesController : ControllerBase
    {
        private readonly MobileCoContext _context;

        public CompaniesController(MobileCoContext context)
        {
            _context = context;
        }

        // GET: api/Companies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyDTO>>> GetCompanies()
        {
            var companies = await _context.Companies
            .Select(a => new CompanyDTO
            {
                ID = a.ID,
                Name = a.Name,
            })
            .OrderBy(c => c.Name)
            .ToListAsync();

            if (companies.Count() > 0)
            {
                return companies;
            }
            else
            {
                return NotFound(new { message = "Error: No Company records" });
            }
        }

        // PUT: api/Companies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompany(int id, CompanyDTO companyDTO)
        {
            if (id != companyDTO.ID)
            {
                return BadRequest(new { message = "Error: ID does not match Company" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Get the record you want to update
            var companyToUpdate = await _context.Companies.FindAsync(id);

            // Check that you got it
            if (companyToUpdate == null)
            {
                return NotFound(new { message = "Error: Company record not found" });
            }

            companyToUpdate.ID = companyDTO.ID;
            companyToUpdate.Name = companyDTO.Name;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(id))
                {
                    return Conflict(new { message = "Concurrency Error: Company has been Removed" });
                }
                else
                {
                    return Conflict(new { message = "Concurrency Error: Company has been updated by another user. Back out and try editing the record again" });
                }
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("UNIQUE"))
                {
                    return BadRequest(new { message = "Unable to save: Duplicate Company name" });
                }
                else
                {
                    return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persists see your system administrator" });
                }
            }
        }

        // POST: api/Companies
        [HttpPost]
        public async Task<ActionResult<CompanyDTO>> PostCompany(CompanyDTO companyDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Company company = new Company
            {
                ID = companyDTO.ID,
                Name = companyDTO.Name,
            };

            try
            {
                _context.Companies.Add(company);
                await _context.SaveChangesAsync();

                // Assign Database Generated values back into the DTO
                companyDTO.ID = company.ID;

                return NoContent();
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("UNIQUE"))
                {
                    return BadRequest(new { message = "Unable to save: Duplicate Company name" });
                }
                else
                {
                    return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persists see your system administrator" });
                }
            }
        }

        // DELETE: api/Companies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var company = await _context.Companies.FindAsync(id);

            if (company == null)
            {
                return NotFound(new { message = "Delete Error: Company has already been removed" });
            }

            try
            {
                _context.Companies.Remove(company);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException)
            {
                return BadRequest(new { message = "Delete Error: Unable to delete Company" });
            }
        }

        private bool CompanyExists(int id)
        {
            return _context.Companies.Any(e => e.ID == id);
        }
    }
}
