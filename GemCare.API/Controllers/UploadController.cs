using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GemCare.API.Common;
using GemCare.API.Contracts.Request;
using GemCare.API.Contracts.Response;
using GemCare.Data.Interfaces;
using GemCare.API.Helper;
namespace GemCare.API.Controllers
{
    public class UploadController : BaseApiController
    {
        private readonly IImageHelper _imageHelper;
        public UploadController(IImageHelper imageHelper)
        {
            _imageHelper = imageHelper;
        }
        [HttpPost("uploadbookingimage")]
        public async Task<IActionResult> UploadBookingImage([FromForm] BookingImageUploadRequest request)
        {
            BookingImageUploadResponse response = new();
            try
            {
                if (IsValidBearerRequest)
                {
                    var documentPath = await _imageHelper.UploadBookingImage(request.bookingimage,request.Userid);
                    //
                    response.Statuscode = System.Net.HttpStatusCode.OK;
                    response.Message = "Success";
                    response.ImagePath = documentPath;
                }
                else
                {
                    response.ToHttpForbiddenResponse(AppConstants.APIKEY_ERRMESSAGE);
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
