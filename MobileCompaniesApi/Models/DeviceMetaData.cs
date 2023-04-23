using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MobileCompaniesApi.Models
{
    public class DeviceMetaData
    {
        [Display(Name = "Device")]
        [Required(ErrorMessage = "You cannot leave the Name blank")]
        [StringLength(128, ErrorMessage = "Name cannot be more than 128 characters long")]
        public string Name { get; set; }

        [Display(Name = "Model")]
        [Required(ErrorMessage = "You cannot leave the Model blank")]
        [StringLength(64, ErrorMessage = "Model cannot be more than 64 characters long")]
        public string Model { get; set; }

        [Display(Name = "Manufacturer")]
        [Required(ErrorMessage = "You cannot leave the Manufacturer blank")]
        [StringLength(128, ErrorMessage = "Manufacturer cannot be more than 128 characters long")]
        public string Manufacturer { get; set; }

        [Display(Name = "Type")]
        [Required(ErrorMessage = "You cannot leave the Type blank")]
        [StringLength(32, ErrorMessage = "Type cannot be more than 32 characters long")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(256)]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }

        [Required(ErrorMessage = "You must select a Company")]
        [Display(Name = "Company")]
        public int CompanyID { get; set; }

    }
}
