using GemCare.API.Contracts.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GemCare.API.Controllers
{
    public class ChatbotController : BaseApiController
    {
        private Dictionary<int, string> chatMessages = new()
        {
            {1, "Hi, welcome to GEM CARE LAB Support." },
            {2, "Please write about the issue you are facing, we are ready to assist you." },
            {3, "Due to large number of requests, our support representatives are occupied at the moment.We will get back to you shortly" },
            {4, "Please forward your concern on following email: support@gemcarelab.co.uk"},
            {5, "Thank you" }
        };

        [HttpGet("message")]
        public IActionResult PostMessage(int messagenumber)
        {
            IBaseResponse response = new BaseResponse();
            if (IsValidBearerRequest)
            {
                response.Statuscode = System.Net.HttpStatusCode.OK;
                response.Message = chatMessages.ElementAt(messagenumber).Value;
            }
            else
            {
                response.ToHttpForbiddenResponse("invalid bearer request");
            }
            return Ok(response);
        }
    }
}
