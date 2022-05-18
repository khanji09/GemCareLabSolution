namespace GemCare.API.Contracts.Response
{
    public class TokenValidationResponse
    {
        public bool Isvalid { get; set; }
        public bool Isexpired { get; set; }
        //public string Message { get; set; }
    }

    public class RefreshTokenResponse
    {
        public string Authtoken { get; set; }
        public string Refreshtoken { get; set; }
    }
}
