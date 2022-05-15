using System;
using System.ComponentModel.DataAnnotations;

namespace GemCare.API.Contracts.Request
{
    public class BookingRequest
    {
        public int Serviceid { get; set; }
        public int Userid { get; set; }      
        public int Addressid { get; set; }
        public string Workdescription { get; set; }
        [Required]
        public DateTime Requireddate { get; set; }
        [Required]
        public string Imagepath { get; set; }
    }
}
