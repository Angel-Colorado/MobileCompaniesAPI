using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MobileCompaniesApi.Models
{
    [ModelMetadataType(typeof(AntennaMetaData))]
    public class Antenna
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int CompanyID { get; set; }
        public Company Company { get; set; }

    }
}