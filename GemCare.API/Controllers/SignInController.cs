using GemCare.API.Common;
using GemCare.API.Contracts.Request;
using GemCare.API.Contracts.Response;
using GemCare.API.Interfaces;
using GemCare.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GemCare.API.Controllers
{
    public class SignInController : BaseApiController
    {
        private readonly IAuthenticate _authenticate;
        private readonly ITokenGenerator _tokenGenerator;
        public SignInController(IAuthenticate authenticate,ITokenGenerator tokenGenerator)
        {
            _authenticate = authenticate;
            _tokenGenerator = tokenGenerator;
        }
        [HttpPost("customerlogin")]
        public IActionResult CustomerSignIn([FromBody] SignInRequest request)
        {
            var response = new SingleResponse<SignInResponse>();
            if (IsValidApiKeyRequest)
            {
                try
                {                   
                    var (status, message, user) = _authenticate.CustomerLogin(request.Email, request.Password);
                    //
                    response.Statuscode = status > 0 ? System.Net.HttpStatusCode.OK :
                        status == -2 ? System.Net.HttpStatusCode.Conflict : System.Net.HttpStatusCode.NotFound;
                    response.Message = message;
                    if (status > 0)
                    {
                        // create user authtoken.
                        response.Result = new SignInResponse
                        {
                            Userid = user.Id,
                            Email = request.Email,
                            Firstname = user.FirstName,
                            Lastname = user.LastName,
                            Authtoken = _tokenGenerator.GenerateAppUserJWT(user.Id, user.UserRole),
                            Refreshtoken = EncHelper.Encrypt(user.Id.ToString()),
                            Imagepath = user.ImagePath,
                            emailcode = user.EmailCode,
                            smsotp = user.SMSOTP
                        };
                    }

                }
                catch (Exception ex)
                {
                    response.ToHttpExceptionResponse(ex.Message);
                }
            }
            else
            {
                response.ToHttpForbiddenResponse(AppConstants.APIKEY_ERRMESSAGE);
            }
            return Ok(response);
        }
    }
}
