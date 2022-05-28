using System;
using System.Collections.Generic;

namespace GemCare.API.PayPal.Business.Response
{



    public class CreateOrderResponse
    {
        public string id { get; set; }
        public string status { get; set; }
        public List<Link> links { get; set; }
    }

    public class Link
    {
        public string href { get; set; }
        public string rel { get; set; }
        public string method { get; set; }
    }


}
