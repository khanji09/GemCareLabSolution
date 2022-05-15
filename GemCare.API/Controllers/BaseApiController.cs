using GemCare.API.Common;
using GemCare.API.Interfaces;
using GemCare.API.Utils;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GemCare.API.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        private const string apiKeyHeaderParam = "apikey";
        private const string bearerHeaderParam = "Authorization";
        private IEncryptionDecryptionHelper _encHelper;
        private IConfiguration _configuration;
        //private ITokenGenerator _tokenGenerator;
        protected IEncryptionDecryptionHelper EncHelper => _encHelper ??= HttpContext.RequestServices.GetService<IEncryptionDecryptionHelper>();
        protected IConfiguration Configuration => _configuration ??= HttpContext.RequestServices.GetService<IConfiguration>();
        protected BaseApiController()
        {
           // _tokenGenerator = tokenGenerator;
        }
        protected bool IsValidApiKeyRequest
        {
            get
            {
                try
                {
                    Request.Headers.TryGetValue(apiKeyHeaderParam, out var apiKey);
                    return EncHelper.IsValidApiKey(apiKey);
                }
                catch { return false; }
                //return false;
            }
        }
        protected bool IsValidBearerRequest
        {

            get
            {
                bool _isValidToken = false;
                try
                {
                    Request.Headers.TryGetValue(bearerHeaderParam, out var authToken);
                    TokenGenerator _tokenGenerator = new TokenGenerator(Configuration, EncHelper);
                    var (isValid, isExpired, message) = _tokenGenerator.ValidateToken(authToken);
                    _isValidToken = isValid;
                }
                catch
                {
                    return false;
                }
                return _isValidToken;
            }
        }
    }

    //public abstract class BaseApiKeyController : BaseApiController
    //{
    //    //protected bool IsValidRequest()
    //    //{
    //    //    return false;
    //    //}
    //}
}
