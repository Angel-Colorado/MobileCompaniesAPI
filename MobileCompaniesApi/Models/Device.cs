using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MobileCompaniesApi.Models
{
    [ModelMetadataType(typeof(DeviceMetaData))]
    public class Device
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public string Type { get; set; }
        public string Username { get; set; }
        public DeviceUserPhoto DeviceUserPhoto { get; set; }
        public int CompanyID { get; set; }
        public Company Company { get; set; }
        public ICollection<Position> Positions { get; set; } = new HashSet<Position>();

    }
}
