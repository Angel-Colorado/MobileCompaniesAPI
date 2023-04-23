using Microsoft.AspNetCore.Mvc;

namespace MobileCompaniesApi.Models
{
    [ModelMetadataType(typeof(CompanyMetaData))]
    public class CompanyDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<AntennaDTO> Antennas { get; set; }
        public ICollection<DeviceDTO> Devices { get; set; }
    }
}
