using GemCare.API.Common;
using GemCare.API.Contracts.Request;
using GemCare.API.Contracts.Response;
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
    public class SignUpController : BaseApiController
    {
        private readonly IAuthenticate _authenticate;
        public SignUpController(IAuthenticate authenticate)
        {
            _authenticate = authenticate;
        }

        [HttpPost("customer")]
        public IActionResult Customer(CustomerRegisterRequest request)
        {
            IBaseResponse response = new BaseResponse();
            if (IsValidApiKeyRequest)
            {
                try
                {
                    CustomerBasicInfo basicInfo = new()
                    {
                        FirstName = request.Firstname,
                        LastName = request.Lastname,
                        Email = request.Email,
                        Password = request.Password,
                        Mobile = request.Mobile
                    };
                    var (status, message) = _authenticate.CustomerRegistration(basicInfo);
                    response.Statuscode = 1 == status ? System.Net.HttpStatusCode.Created : System.Net.HttpStatusCode.ExpectationFailed;
                    response.Message = message;
                }
                catch(Exception ex)
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
