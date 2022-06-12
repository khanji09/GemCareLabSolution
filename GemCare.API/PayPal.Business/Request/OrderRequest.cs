
using System.Collections.Generic;

namespace GemCare.API.PayPal.Business.Request
{
    public class CreateOrderRequest
    {
        public string intent { get; set; }
        public string processing_instruction { get; set; }
        public Purchase_Units[] purchase_units { get; set; }
        public Application_Context application_context { get; set; }
    }

    public class Application_Context
    {
        public string return_url { get; set; }
        public string cancel_url { get; set; }
    }

    public class Purchase_Units
    {
        public Item[] items { get; set; }
        public Amount amount { get; set; }
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

    public class Item
    {
        public string name { get; set; }
        public string description { get; set; }
        public string quantity { get; set; }
        public Unit_Amount unit_amount { get; set; }
    }

    public class Unit_Amount
    {
        public string currency_code { get; set; }
        public string value { get; set; }
    }

    //*******************************//
    // capture payment request
    //******************************//
    public class PayPalCapturePaymentRequest
    {
        public string Orderid { get; set; }
        public string Token { get; set; }
        public string Payerid { get; set; }
        public string PayPalrequestid { get; set; }

    }

    public class PayPalCapturePaymentRequest_New : PayPalCapturePaymentRequest
    {
        public int Bookingid { get; set; }
        public double Amount { get; set; }
        public string Currencycode { get; set; }
    }

    public class AuthorizePaymentRequest
    {
        public string OrderId { get; set; }
        public string PayPalRequestId { get; set; }

    }
}