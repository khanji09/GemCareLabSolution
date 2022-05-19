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
    public class SignInController : BaseApiController
    {
        private readonly IAuthenticate _authenticate;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IEmailService _emailService;
        public SignInController(IAuthenticate authenticate,ITokenGenerator tokenGenerator, IEmailService emailService)
        {
            _authenticate = authenticate;
            _tokenGenerator = tokenGenerator;
            _emailService = emailService;
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
                            Email = user.Email,
                            Firstname = user.FirstName,
                            Lastname = user.LastName,
                           // Authtoken = _tokenGenerator.GenerateAppUserJWT(user.Id, user.UserRole),
                           // Refreshtoken = EncHelper.Encrypt(user.Id.ToString()),
                            Imagepath = user.ImagePath,
                            emailcode = user.EmailCode,
                            smsotp = user.SMSOTP
                        };
                      bool emailSent=  _emailService.SendLoginCode(user.Email, user.EmailCode.ToString());
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

        [HttpPost("VerifyLogin")]
        public IActionResult VerifyLogin([FromBody] EmailLoginCodeVerificationRequest request)
        {
            var response = new SingleResponse<SignInResponse>();
            if (IsValidApiKeyRequest)
            {
                try
                {
                    EmailLoginCodeDTO dto = new EmailLoginCodeDTO() { EmailCode=request.Emailcode,UserId=request.Userid};
                    var (status, message, user) = _authenticate.VerifyEmailLoginCode(dto);
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
                            Email = user.Email,
                            Firstname = user.FirstName,
                            Lastname = user.LastName,
                            Authtoken = _tokenGenerator.GenerateAppUserJWT(user.Id, user.UserRole),
                            Refreshtoken = EncHelper.Encrypt(user.Id.ToString()),
                            Imagepath = user.ImagePath                            
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

        [HttpPost("admin")]
        public IActionResult AdminSignIn([FromBody] AdminSignInRequest request)
        {
            var response = new SingleResponse<AdminSignInResponse>();
            if (IsValidApiKeyRequest)
            {
                try
                {
                    var (status, message, user) = _authenticate.AdminLogin(request.Email, request.Password);
                    //
                    response.Statuscode = status > 0 ? System.Net.HttpStatusCode.OK :
                        status == -2 ? System.Net.HttpStatusCode.Conflict : System.Net.HttpStatusCode.NotFound;
                    response.Message = message;
                    if (status > 0)
                    {
                        // create user authtoken.
                        response.Result = new AdminSignInResponse
                        {
                            Userid = user.Id,
                            Email = request.Email,
                            Firstname = user.FirstName,
                            Lastname = user.LastName,
                            Authtoken = _tokenGenerator.GenerateAppUserJWT(user.Id, user.UserRole),
                            Refreshtoken = EncHelper.Encrypt(user.Id.ToString()),
                            Imagepath = user.ImagePath
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
        
        [HttpPost("generatenewtoken")]
        public IActionResult GenerateNewToken([FromBody] RefreshTokenRequest request)
        {
            var response = new SingleResponse<RefreshTokenResponse>();
            try
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
            catch (Exception ex)
            {
                response.ToHttpExceptionResponse(ex.Message);
            }
            return Ok(response);
        }
    }
}
