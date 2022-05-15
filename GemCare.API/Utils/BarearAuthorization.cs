using GemCare.API.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GemCare.API.Utils
{
    public class BarearAuthorization: IAuthorizationFilter
    {
        private readonly ITokenGenerator _tokenGenerator;
        readonly string[] exemptedUrls =
        {
            "/User/profileimage"
        };
        public BarearAuthorization(ITokenGenerator tokenGenerator)
        {
            _tokenGenerator = tokenGenerator;
        }
        public bool IsAuthorized(string authToken)
        {
            var (isValid, isExpired, message) = _tokenGenerator.ValidateToken(authToken);
            return isValid && !isExpired;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}
