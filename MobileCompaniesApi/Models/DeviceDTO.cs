using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MobileCompaniesApi.Models
{
    [ModelMetadataType(typeof(DeviceMetaData))]
    public class DeviceDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public string Type { get; set; }
        public string Username { get; set; }
        public DeviceUserPhotoDTO DeviceUserPhoto { get; set; }
        public int CompanyID { get; set; }
        public CompanyDTO Company { get; set; }
        public ICollection<PositionDTO> Positions { get; set; }

    }
}
