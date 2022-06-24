using System;
using System.Collections.Generic;

namespace GemCare.API.PayPal.Business.Response
{



    //public class CreateOrderResponse
    //{
    //    public string id { get; set; }
    //    public string status { get; set; }
    //    public string PayPalRequestId { get; set; }
    //    public List<Link> links { get; set; }
    //}

    //public class Link
    //{
    //    public string href { get; set; }
    //    public string rel { get; set; }
    //    public string method { get; set; }
    //}

    ////

    public class CreateOrderResponse
    {
        public string id { get; set; }
        public string intent { get; set; }
        public string status { get; set; }
        public string PayPalRequestId { get; set; }
        public string processing_instruction { get; set; }
        public Purchase_Units[] purchase_units { get; set; }
        public DateTime create_time { get; set; }
        public Link[] links { get; set; }
        public PayPalError Error { get; set; }
    }

    public class Purchase_Units
    {
        public string reference_id { get; set; }
        public Amount amount { get; set; }
        public Payee payee { get; set; }
        public Item[] items { get; set; }
    }

    public class Amount
    {
        public string currency_code { get; set; }
        public string value { get; set; }
        public Breakdown breakdown { get; set; }
    }

    public class Breakdown
    {
        public Item_Total item_total { get; set; }
    }

    public class Item_Total
    {
        public string currency_code { get; set; }
        public string value { get; set; }
    }

    public class Payee
    {
        public string email_address { get; set; }
        public string merchant_id { get; set; }
    }

    public class Item
    {
        public string name { get; set; }
        public Unit_Amount unit_amount { get; set; }
        public string quantity { get; set; }
        public string description { get; set; }
    }

    public class Unit_Amount
    {
        public string currency_code { get; set; }
        public string value { get; set; }
    }

    public class Link
    {
        public string href { get; set; }
        public string rel { get; set; }
        public string method { get; set; }
    }

    public class PayPalError
    {
        public string name { get; set; }
        public string message { get; set; }
        public string debug_id { get; set; }
    }

}
