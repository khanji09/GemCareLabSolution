using GemCare.API.Common;
using GemCare.API.Contracts.Request;
using GemCare.API.Contracts.Response;
using GemCare.API.Interfaces;
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
        private readonly IPushService _pushService;
        public PushNotificationController(IPushNotificationRepository pushNotificationRepository, IPushService pushService)
        {
            _pushRepository = pushNotificationRepository;
            _pushService = pushService;
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
            return Ok(response);
        }

        [HttpPost("sendnotification")]
        public IActionResult SendNotification()
        {
            IBaseResponse response = new BaseResponse();
            try
            {
                var result = _pushService.SendBookingNotificationToAdmin("test notification", "test notification from api",
                    "c6u98DVHTTi1Hzl-AbreG_:APA91bHJ4BpezQR5Z2M4TGxOBHQNT2xly7MP82NN6REJsCNZ-efNVSB8RrU7MK5pbqWaDnx9yXqt6btorWwj64BnSGjCiJJd5oBEH_IZutpD0SBMH24Qp_uYmV4EQM5AhC8Ir758Rsiq", "android");
                if (result.notificationSent)
                {
                    response.Statuscode = System.Net.HttpStatusCode.OK;
                    response.Message = result.errMessage;
                }
            }
            catch (Exception ex)
            {
                response.ToHttpExceptionResponse(ex.Message);
            }
            return Ok(response);
        }
    }
}
