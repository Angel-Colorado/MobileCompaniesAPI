using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobileCompaniesApi.Data;
using MobileCompaniesApi.Models;
using MobileCompaniesApi.Utilities;
using Newtonsoft.Json.Linq;
using SkiaSharp;

namespace MobileCompaniesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly MobileCoContext _context;

        public DevicesController(MobileCoContext context)
        {
            _context = context;
        }

        // GET: api/Devices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeviceDTO>>> GetDevices()
        {
            var deviceDTOs = await _context.Devices
                .Select(d => new DeviceDTO
                {
                    ID = d.ID,
                    Name = d.Name,
                    Model = d.Model,
                    Manufacturer = d.Manufacturer,
                    Type = d.Type,
                    Username = d.Username,
                    CompanyID = d.CompanyID,
                })
                .ToListAsync();

            return deviceDTOs;
        }

        // Determines if the device exists in the DB, if so returns it
        [HttpGet("Exists")]
        public async Task<ActionResult<DeviceDTO>> GetDeviceExists(string Name, string Model, string Manufacturer, string Type)
        {
            var deviceToFind = await _context.Devices
                .Include(c => c.Company)
                .Include(p => p.DeviceUserPhoto)
                .Select(d => new DeviceDTO
                {
                    ID = d.ID,
                    Name = d.Name,
                    Model = d.Model,
                    Manufacturer = d.Manufacturer,
                    Type = d.Type,
                    Username = d.Username,
                    CompanyID = d.CompanyID,
                    Company = new CompanyDTO
                    {
                        ID = d.Company.ID,
                        Name = d.Company.Name
                    }
                })
                .FirstOrDefaultAsync(f => f.Name == Name && f.Model == Model && f.Manufacturer == Manufacturer && f.Type == Type);

            if (deviceToFind == null)
            {
                return NotFound(new { message = "Error: This device doesn't exist in the Database" });
            }

            // Checks if the device has an image
            var imageToFind = await _context.DeviceUserPhotos
                .Select(d => new DeviceUserPhotoDTO
                {
                    ID = d.ID,
                    Content = d.Content,
                    MimeType = d.MimeType,
                    DeviceID = d.DeviceID
                })
                .FirstOrDefaultAsync(p => p.DeviceID == deviceToFind.ID);

            // If there is an image, the it will add it to the DeviceDTO
            if (imageToFind != null)
            {
                deviceToFind.DeviceUserPhoto = imageToFind;
            }

            return deviceToFind;

        }

        // UPDATE: api/Devices/5
        [HttpPut]
        public async Task<IActionResult> PutDevice(int ID, string Username, int CompanyID, bool RemoveImage)
        {
            var deviceToUpdate = await _context.Devices
                .Include(p => p.DeviceUserPhoto)
                .FirstOrDefaultAsync(d => d.ID == ID);

            if (deviceToUpdate == null)
            {
                return NotFound();
            }

            deviceToUpdate.Username = Username;
            deviceToUpdate.CompanyID = CompanyID;

            try
            {
                if (RemoveImage)    // Removes the image
                {
                    deviceToUpdate.DeviceUserPhoto = null;
                }
                else                // Otherwise, gets the file and creates the image
                {
                    HttpRequest httpRequest = HttpContext.Request;

                    if (httpRequest.Form.Files.Count > 0)
                    {
                        var file = httpRequest.Form.Files.FirstOrDefault();

                        await CreatePicture(deviceToUpdate, file);
                    }
                }

                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeviceExists(ID))
                {
                    return Conflict(new { message = "Concurrency Error: Device has been Removed" });
                }
                else
                {
                    return Conflict(new { message = "Concurrency Error: Device has been updated by another user. Back out and try editing the record again" });
                }
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("UNIQUE"))
                {
                    return BadRequest(new { message = "Unable to save: Duplicate Device info on: Name, Model, Manufacturer and Type" });
                }
                else
                {
                    return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persists see your system administrator" });
                }
            }
        }

        // CREATE: api/Devices
        [HttpPost]
        public async Task<ActionResult<DeviceDTO>> PostDevice(string Name, string Model,
                string Manufacturer, string Type, string Username, int CompanyID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Device device = new Device
            {
                Name = Name,
                Model = Model,
                Manufacturer = Manufacturer,
                Type = Type,
                Username = Username,
                CompanyID = CompanyID
            };

            try
            {
                HttpRequest httpRequest = HttpContext.Request;

                if (httpRequest.Form.Files.Count > 0)
                {
                    var file = httpRequest.Form.Files.FirstOrDefault();

                    await CreatePicture(device, file);
                }

                _context.Devices.Add(device);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("UNIQUE"))
                {
                    return BadRequest(new { message = "Unable to save: Duplicate Device info on: Name, Model, Manufacturer and Type" });
                }
                else
                {
                    return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persists see your system administrator" });
                }
            }
        }

        // DELETE: api/Devices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDevice(int id)
        {
            var device = await _context.Devices.FindAsync(id);

            if (device == null)
            {
                return NotFound(new { message = "Delete Error: Device has already been removed" });
            }

            try
            {
                _context.Devices.Remove(device);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException)
            {
                return BadRequest(new { message = "Delete Error: Unable to delete Device" });
            }
        }

        private async Task CreatePicture(Device deviceToUpdate, IFormFile thePicture)
        {
            // Gets the picture and save it
            if (thePicture != null)
            {
                string mimeType = thePicture.ContentType;
                long fileLength = thePicture.Length;

                if (!(mimeType == "" || fileLength == 0)) // Looks like we have a file
                {
                    using var stream = thePicture.OpenReadStream();
                    using var memoryStream = new MemoryStream();
                    await stream.CopyToAsync(memoryStream);
                    var pictureArray = memoryStream.ToArray();  // Gives us the Byte[]

                    deviceToUpdate.DeviceUserPhoto = new DeviceUserPhoto
                    {
                        Content = ResizeImage.shrinkImage(pictureArray, 500, 600, SKEncodedImageFormat.Png),
                        MimeType = "image/png"
                    };
                }
            }
        }

        private bool DeviceExists(int id)
        {
            return _context.Devices.Any(e => e.ID == id);
        }
    }
}
