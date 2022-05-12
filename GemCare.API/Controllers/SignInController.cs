using GemCare.API.Common;
using GemCare.API.Contracts.Request;
using GemCare.API.Contracts.Response;
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
        public SignInController(IAuthenticate authenticate)
        {
            _authenticate = authenticate;
        }
        [HttpPost("customer")]
        public IActionResult CustomerSignIn([FromBody] SignInRequest request)
        {
            var response = new SingleResponse<SignInResponse>();
            if (IsValidApiKeyRequest)
            {
                try
                {
                    // response status code for tutor signin.
                    // 200 : Ok and redirect to inner screen
                    // 400 : Bad Request
                    // 409 : Account not confirmed. Frontend will redirect tutor to account confirmation code screen.
                    // 404 : Either email or password is incorrect. Frontend will show the returned message.
                    // tutor login
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
                            Authtoken = string.Empty, //TokenGenerator.GenerateAppUserJWT(user.Id, user.UserRole),
                            Refreshtoken = EncHelper.Encrypt(user.Id.ToString()),
                            Imagepath = user.ImagePath
                        };
                    }
                    //else
                    //{
                    //    if(status == -2)
                    //    {
                    //        // account not confirmed. User will be redirected to account
                    //        // confirmation screen.
                    //        response.Statuscode = System.Net.HttpStatusCode.Conflict;
                    //    }
                    //}
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
