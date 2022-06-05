using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GemCare.API.Contracts.Response
{
    public class ValuationBookingResponse
    {
        public int Id { get; set; }
        public int Customerid { get; set; }
        public string Customerfirstname { get; set; }
        public string Customerlastname { get; set; }
        public int Technicianid { get; set; }
        public string Techfirstname { get; set; }
        public string Techlastname { get; set; }
        public string Itemdescription { get; set; }
        public string Servicename { get; set; }
        public string Imageurl { get; set; }
        public string Videourl { get; set; }
        public int Quotation { get; set; }
    }
}
