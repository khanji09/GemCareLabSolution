namespace GemCare.API.Contracts.Request
{
    public class PayPalCreateOrderRequest
    {
     
        public int BookingId { get; set; }
        public double Amount { get; set; }
        public string Currencycode { get; set; }
        public string Returnurl { get; set; }
        public string Cancelurl { get; set; }
    }
}
