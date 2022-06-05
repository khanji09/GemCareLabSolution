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
using Microsoft.AspNetCore.Cors;
using GemCare.API.Interfaces;

namespace GemCare.API.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IImageHelper _imageHelper;
        private readonly ITokenGenerator _tokenGenerator;
        public UploadController(IImageHelper imageHelper, ITokenGenerator tokenGenerator)
        {
            _imageHelper = imageHelper;
            _tokenGenerator = tokenGenerator;
        }

        [HttpPost("uploadbookingimage")]
        public async Task<IActionResult> UploadBookingImage([FromForm] BookingImageUploadRequest request)
        {
            BookingImageUploadResponse response = new();
            try
            {
                if (_tokenGenerator.ValidateToken(request.Authtoken).isValid)
                {
                    var documentPath = await _imageHelper.UploadBookingImage(request.bookingimage,request.Userid);
                    //
                    response.Statuscode = System.Net.HttpStatusCode.OK;
                    response.Message = "Success";
                    response.ImagePath = documentPath;
                }
                else
                {
                    response.ToHttpForbiddenResponse(AppConstants.BEARER_ERRMESSAGE);
                }
            }
            catch (Exception ex)
            {
                response.ToHttpExceptionResponse(ex.Message);
            }
            return Ok(response);
        }

        [HttpPost("uploadprofileimage")]
        public async Task<IActionResult> UploadProfileImage([FromForm] UserProfileImageRequest request)
        {
            ProfileImageUploadResponse response = new();
            try
            {
                if (_tokenGenerator.ValidateToken(request.Authtoken).isValid)
                {
                    var imagePath = await _imageHelper.UploadProfileImage(request.Profileimage, request.Userid);
                    //
                    response.Statuscode = System.Net.HttpStatusCode.OK;
                    response.Message = "Success";
                    response.ImagePath = imagePath;
                }
                else
                {
                    response.ToHttpForbiddenResponse(AppConstants.BEARER_ERRMESSAGE);
                }
            }
            catch (Exception ex)
            {
                response.ToHttpExceptionResponse(ex.Message);
            }
            return Ok(response);
        }

        [HttpPost("uploadvaluationimage")]
        public async Task<IActionResult> UploadValuationImage([FromForm] ValuationImageRequest request)
        {
            ProfileImageUploadResponse response = new();
            try
            {
                if (_tokenGenerator.ValidateToken(request.Authtoken).isValid)
                {
                    var imagePath = await _imageHelper.UploadValuationImage(request.file, request.Userid);
                    //
                    response.Statuscode = System.Net.HttpStatusCode.OK;
                    response.Message = "Success";
                    response.ImagePath = imagePath;
                }
                else
                {
                    response.ToHttpForbiddenResponse(AppConstants.BEARER_ERRMESSAGE);
                }
            }
            catch (Exception ex)
            {
                response.ToHttpExceptionResponse(ex.Message);
            }
            return Ok(response);
        }

        [HttpPost("uploadvaluationvideo")]
        public async Task<IActionResult> UploadValuationVideo([FromForm] ValuationImageRequest request)
        {
            ProfileImageUploadResponse response = new();
            try
            {
                if (_tokenGenerator.ValidateToken(request.Authtoken).isValid)
                {
                    var imagePath = await _imageHelper.UploadValuationImage(request.file, request.Userid);
                    //
                    response.Statuscode = System.Net.HttpStatusCode.OK;
                    response.Message = "Success";
                    response.ImagePath = imagePath;
                }
                else
                {
                    response.ToHttpForbiddenResponse(AppConstants.BEARER_ERRMESSAGE);
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
