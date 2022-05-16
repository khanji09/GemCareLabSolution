namespace GemCare.API.Contracts.Request
{
    public class EmailLoginCodeVerificationRequest
    {
        public int Userid { get; set; }
        public int Emailcode { get; set; }
    }
}
