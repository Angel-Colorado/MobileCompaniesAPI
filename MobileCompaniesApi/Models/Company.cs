using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MobileCompaniesApi.Models
{
    [ModelMetadataType(typeof(CompanyMetaData))]
    public class Company
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<Antenna> Antennas { get; set; } = new HashSet<Antenna>();
        public ICollection<Device> Devices { get; set; } = new HashSet<Device>();

    }
}
