namespace GemCare.API.PayPal.Business
{
    public class PayPalConfiguration
    {
        public PayPalConfiguration()
        {

        }
        public string ClientId { get; set; }
        public string SecretKey { get; set; }

        public string Url { get; set; }
    }
}
