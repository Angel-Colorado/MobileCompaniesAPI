using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MobileCompaniesApi.Models
{
    public class AntennaMetaData
    {
        [Display(Name = "Antenna")]
        [Required(ErrorMessage = "You cannot leave the Name blank")]
        [StringLength(100, ErrorMessage = "Name cannot be more than 100 characters long")]
        public string Name { get; set; }

        [Display(Name = "Latitude")]
        [Required(ErrorMessage = "You cannot leave the Latitude blank")]
        public decimal Latitude { get; set; }

        [Display(Name = "Longitude")]
        [Required(ErrorMessage = "You cannot leave the Longitude blank")]
        public decimal Longitude { get; set; }

        [Display(Name = "Description")]
        [StringLength(256, ErrorMessage = "Longitude cannot be more than 256 characters long")]
        public string Description { get; set; }

        [Display(Name = "Image")]
        [StringLength(1024, ErrorMessage = "URL Image cannot be more than 1,024 characters long")]
        public string Image { get; set; }

        [Required(ErrorMessage = "You must select a Company")]
        [Display(Name = "Company")]
        public int CompanyID { get; set; }

    }
}
