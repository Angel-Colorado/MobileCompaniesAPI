using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MobileCompaniesApi.Models
{
    public class PositionMetaData
    {
        [Display(Name = "G Maps link")]
        public string Link
        {
            get
            {
                return $"https://maps.google.com/?q={Latitude},{Longitude}";
            }
        }

        [Display(Name = "Date")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime TimeStamp { get; set; } = DateTime.Now;

        [Display(Name = "Latitude")]
        [Required(ErrorMessage = "You cannot leave the Latitude blank")]
        public decimal Latitude { get; set; }

        [Display(Name = "Longitude")]
        [Required(ErrorMessage = "You cannot leave the Longitude blank")]
        public decimal Longitude { get; set; }

        [Required(ErrorMessage = "You must select a Device")]
        [Display(Name = "Device")]
        public int DeviceID { get; set; }
        public Device Device { get; set; }

    }
}
