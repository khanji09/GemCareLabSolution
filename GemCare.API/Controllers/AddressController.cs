using GemCare.API.Common;
using GemCare.API.Contracts.Response;
using GemCare.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GemCare.API.Controllers
{
    public class AddressController : BaseApiController
    {
        private readonly IAddressService _addressService;
        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet("postalcodeaddresses")]
        public async Task<IActionResult> GetPostalCodeAddresses(string postalcode)
        {
            ISingleResponse<PostalCodeAddressResponse> response = new SingleResponse<PostalCodeAddressResponse>
            {
                Statuscode = System.Net.HttpStatusCode.NotFound,
                Message = "No data found",
                Result = new PostalCodeAddressResponse()
            };
            if (IsValidBearerRequest)
            {
                try
                {
                    var result = await _addressService.GetPostalCodeAddresses(postalcode);
                    if(result != null)
                    {
                        response.Statuscode = System.Net.HttpStatusCode.OK;
                        response.Message = AppConstants.SUCCESS_MESSAGE;
                        response.Result.Addresses = result;
                    }
                }
                catch (Exception ex)
                {
                    response.ToHttpExceptionResponse(ex.Message);
                }
            }
            else
            {
                response.ToHttpForbiddenResponse(AppConstants.BEARER_ERRMESSAGE);
            }
            return Ok(response);
        }
    }
}
