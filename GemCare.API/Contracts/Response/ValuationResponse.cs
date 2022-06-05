using System.Collections.Generic;

namespace GemCare.API.Contracts.Response
{
    public class ValuationResponse
    {
       public int valuationid { get; set; }
    }

    public class AdminValuationResponse
    {
        public int Id { get; set; }
        public int Customerid { get; set; }
        public string Customerfirstname { get; set; }
        public string Customerlastname { get; set; }
        public int Technicianid { get; set; }
        public string Techfirstname { get; set; }
        public string Techlastname { get; set; }
        public string Servicename { get; set; }
        public string Itemdescription { get; set; }
        public string Imageurl { get; set; }
        public string Videourl { get; set; }
        public int Quotation { get; set; }
    }
    public class AdminValuationListResponse
    {
        public List<AdminValuationResponse> ValuationsList { get; set; }
    }
}
