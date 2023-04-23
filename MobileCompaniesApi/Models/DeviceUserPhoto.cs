using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;

namespace MobileCompaniesApi.Models
{
    [ModelMetadataType(typeof(DeviceUserPhotoMetaData))]
    public class DeviceUserPhoto
    {
        public int ID { get; set; }
        public byte[] Content { get; set; }
        public string MimeType { get; set; }
        public int DeviceID { get; set; }
        public Device Device { get; set; }

    }
}
