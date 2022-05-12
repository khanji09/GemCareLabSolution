using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GemCare.API.Controllers
{
    public class ServicesController : BaseApiController
    {
        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            return Ok();
        }
    }
}
