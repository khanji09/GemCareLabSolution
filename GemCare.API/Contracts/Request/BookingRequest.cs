using System;
using System.ComponentModel.DataAnnotations;

namespace GemCare.API.Contracts.Request
{
    public class BookingRequest
    {
        public int Serviceid { get; set; }
        public int Userid { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Postalcode { get; set; }
        public string Mobilenumber { get; set; }
        public string Workdescription { get; set; }
        [Required]
        public DateTime Requireddate { get; set; }
        [Required]
        public string Imagepath { get; set; }
    }
}
