using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MobileCompaniesApi.Models
{
    [ModelMetadataType(typeof(PositionMetaData))]
    public class PositionDTO
    {
        public int ID { get; set; }
        public string Link
        {
            get
            {
                return $"https://maps.google.com/?q={Latitude},{Longitude}";
            }
        }
        public DateTime TimeStamp { get; set; } = DateTime.Now;
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int DeviceID { get; set; }
        public DeviceDTO Device { get; set; }

    }
}
