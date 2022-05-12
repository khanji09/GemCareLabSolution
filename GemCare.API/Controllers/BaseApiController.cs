using GemCare.API.Utils;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        //private ITokenGenerator _tokenGenerator;
        protected IEncryptionDecryptionHelper EncHelper => _encHelper ??= HttpContext.RequestServices.GetService<IEncryptionDecryptionHelper>();

        protected BaseApiController()
        {
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
                try
                {
                    Request.Headers.TryGetValue(bearerHeaderParam, out var authToken);
                }
                catch
                {
                    return false;
                }
                return false;
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
