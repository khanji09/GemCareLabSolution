using System;

namespace GemCare.API.Contracts.Request
{
    public class BookingRequest
    {
        public int Serviceid { get; set; }
        public int Userid { get; set; }      
        public int Addressid { get; set; }
        public string Workdescription { get; set; }       
        public DateTime Requireddate { get; set; }      
        public string Imagepath { get; set; }
    }
}
