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
    public class PushNotificationController : BaseApiController
    {
        private readonly IPushNotificationRepository _pushRepository;
        public PushNotificationController(IPushNotificationRepository pushNotificationRepository)
        {
            _pushRepository = pushNotificationRepository;
        }

        [HttpPost("savepushtoken")]
        public IActionResult SavePushToken([FromBody] PushNotificationSaveRequest request)
        {
            IBaseResponse response = new BaseResponse();
            if(IsValidBearerRequest)
            {
                try
                {
                    var (status, message) = _pushRepository.SavePushToken(User_Id, request.Pushtoken, request.Deviceid,
                        request.Deviceplatform);
                    response.Statuscode = status > 0 ? System.Net.HttpStatusCode.OK
                       : System.Net.HttpStatusCode.NotFound;
                    response.Message = message;
                }
                catch (Exception ae)
                {
                    response.ToHttpExceptionResponse(ae.Message);
                }
            }
            else
            {
                response.ToHttpForbiddenResponse(AppConstants.BEARER_ERRMESSAGE);
            }
            return Ok();
        }
    }
}
