using GemCare.API.Contracts.Response;
using GemCare.API.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GemCare.API.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class PolicyController : ControllerBase
    {
        [HttpGet("useragreement")]
        public IActionResult GetUserAgreement()
        {
            UserAgreementResponse response = new();
            try
            {
                var complianceData = UserCompliance.GetUserAgreement();
                response.Statuscode = !string.IsNullOrEmpty(complianceData) ? System.Net.HttpStatusCode.OK
                    : System.Net.HttpStatusCode.NotFound;
                response.Message = !string.IsNullOrEmpty(complianceData) ? "Success" : "No data found";
                response.Agreementtext = complianceData;
            }
            catch (Exception ex)
            {
                response.ToHttpExceptionResponse(ex.Message);
            }
            return Ok(response);
        }
    }
}
