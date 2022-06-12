using Microsoft.AspNetCore.Http;

namespace GemCare.API.Contracts.Request
{
    public class ValuationRequest
    {
       // public int Id { get; set; }
        public int Customerid { get; set; }
        public int Technicianid { get; set; }
        public int ServiceId { get; set; }
        public string Itemdescription { get; set; }
        public string Imagepath { get; set; }
        public string Videopath { get; set; }
        public int Quotation { get; set; }
       
    }

    public class ValuationImageRequest : BaseAuthTokenRequest
    {
        public int Userid { get; set; }
        public IFormFile file { get; set; }
    }

    public class AssignValuationRequest
    {
        public int Id { get; set; }
        public int Technicianid { get; set; }
    }
}
