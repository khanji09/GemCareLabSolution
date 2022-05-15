using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GemCare.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : BaseApiController
    {
        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            if (IsValidApiKeyRequest)
            {

            }
            else
            {

            }
            return Ok();
        }

        [HttpGet("detail")]
        public IActionResult ServiceDetail(int serviceid)
        {
            return Ok();
        }
    }
}
