namespace GemCare.API.Contracts.Request
{
    public class ValidateRequest //: BaseApiKeyRequest
    {
        public string Authtoken { get; set; }
    }

    public class RefreshTokenRequest : ValidateRequest
    {
        public string Refreshtoken { get; set; }
    }
}
