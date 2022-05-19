using GemCare.API.Common;
using GemCare.API.Contracts.Request;
using GemCare.API.Contracts.Response;
using GemCare.API.Interfaces;
using GemCare.Data.DTOs;
using GemCare.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GemCare.API.Controllers
{
    public class AuthTokenController : BaseApiController
    {
        
        private readonly ITokenGenerator _tokenGenerator;
      
        public AuthTokenController(ITokenGenerator tokenGenerator)
        {
            
            _tokenGenerator = tokenGenerator;
            
        }
       

        [HttpPost("Refreshtoken")]
        public IActionResult RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var response = new SingleResponse<RefreshTokenResponse>();
            try
            {
                if (IsValidApiKeyRequest)
                {
                    var authToken = _tokenGenerator.GenerateNewToken(request.Authtoken, request.Refreshtoken);
                    response.Statuscode = System.Net.HttpStatusCode.OK;
                    response.Message = "Success";
                    response.Result = new()
                    {
                        Authtoken = authToken,
                        Refreshtoken = request.Refreshtoken
                    };
                }
                else
                {
                    response.ToHttpForbiddenResponse(AppConstants.APIKEY_ERRMESSAGE);
                }
            }
            catch (Exception ex)
            {
                response.ToHttpExceptionResponse(ex.Message);
            }
            return Ok(response);
        }

        [HttpPost("Validatetoken")]
        public IActionResult ValidateToken([FromBody] ValidateRequest request)
        {
            var response = new SingleResponse<TokenValidationResponse>();
            try
            {
                if (IsValidApiKeyRequest)
                {
                    var (isValid, isExpired, message) = _tokenGenerator.ValidateToken(request.Authtoken);
                   
                    response.Statuscode = isValid ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.Unauthorized;
                    response.Message = message;
                    response.Result = new()
                    {
                        Isvalid = isValid,
                        Isexpired = isExpired
                    };
                }
               else {
                    response.ToHttpForbiddenResponse(AppConstants.APIKEY_ERRMESSAGE);
                }
            }
            catch (Exception ex)
            {
                response.ToHttpExceptionResponse(ex.Message);
            }
            return Ok(response);
        }

    }
}
