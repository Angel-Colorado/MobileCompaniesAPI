using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MobileCompaniesApi.Models
{
    public class CompanyMetaData
    {
        [Display(Name = "Company")]
        [Required(ErrorMessage = "You cannot leave the Company Name blank")]
        [StringLength(100, ErrorMessage = "Company Name cannot be more than 100 characters long")]
        public string Name { get; set; }

        [Display(Name = "Antennas")]
        public ICollection<Antenna> Antennas { get; set; } = new HashSet<Antenna>();

        [Display(Name = "Devices")]
        public ICollection<Device> Devices { get; set; } = new HashSet<Device>();

    }
}
