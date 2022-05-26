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

namespace GemCare.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayPalController : BaseApiController
    {
        
        private readonly ITokenGenerator _tokenGenerator;
        public PayPalController(ITokenGenerator tokenGenerator)
        {            
            _tokenGenerator = tokenGenerator;
        }
        [HttpGet("getaccesstoken")]
        public IActionResult getAccessToken()
        {
            var response = new BaseResponse();                
            response.Message=PayPal.Business.AccessToken.GetAccessTokenWithBearer();
            return Ok(response);
        }

       
    }
}
