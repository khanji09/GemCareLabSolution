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
    public class CustomerController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        //private readonly ITokenGenerator _tokenGenerator;
        public CustomerController(IUserRepository userRepository, ITokenGenerator tokenGenerator)
        {
            _userRepository = userRepository;
           // _tokenGenerator = tokenGenerator;
        }
        [HttpGet("GetProfile")]
        public IActionResult GetProfile()
        {
            var response = new SingleResponse<UserProfileResponse>()
            {
                Result = new UserProfileResponse()
            };
            try
            {
                if (IsValidBearerRequest)
                {

                    (int status, string message, UserProfileDTO profile) = _userRepository.GetUserPRofile(User_Id);
                    response.Statuscode = status > 0 ? System.Net.HttpStatusCode.OK :
                       status == -2 ? System.Net.HttpStatusCode.Conflict : System.Net.HttpStatusCode.NotFound;
                    response.Message = message;
                    response.Result = new UserProfileResponse()
                    {
                        Dateofbirth = profile.DOB,
                        Email = profile.Email,
                        Firstname = profile.FirstName,
                        Gender = profile.Gender,
                        Id = profile.Id,
                        Imagepath = profile.ImagePath,
                        Lastname = profile.LastName,
                        Mobile = profile.Mobile
                    };
                }
                else
                {
                    response.ToHttpForbiddenResponse(AppConstants.BEARER_ERRMESSAGE);
                }
            }
            catch (Exception ae)
            {
                response.Message = ae.Message;
                response.Haserror = true;
            }

            return Ok(response);
        }

        [HttpPost("UpdateProfile")]
        public IActionResult UpdateProfile(UserProfileUpdateRequest model)
        {
            var response = new SingleResponse<BaseResponse>()
            {
                Result = new BaseResponse()
            };

            try
            {
                if (IsValidBearerRequest)
                {
                    UserProfileDTO _dto = new UserProfileDTO()
                    {
                        Gender = model.Gender,
                        LastName = model.LastName,
                        FirstName = model.FirstName,
                        DOB = model.DOB,
                        Mobile = model.Mobile,
                        Id = User_Id
                    };
                    (int status, string message) = _userRepository.UpdateUserPRofile(_dto);
                    response.Statuscode = status > 0 ? System.Net.HttpStatusCode.OK :
                       status == -2 ? System.Net.HttpStatusCode.Conflict : System.Net.HttpStatusCode.NotFound;
                    response.Message = message;

                }
                else
                {
                    response.ToHttpForbiddenResponse(AppConstants.BEARER_ERRMESSAGE);
                }
            }
            catch (Exception ae)
            {
                response.Message = ae.Message;
                response.Haserror = true;
            }

            return Ok(response);
        }

        [HttpPost("UpdateProfileImage")]

        public IActionResult UpdateProfileImage(UserProfileUpdateImageRequest model)
        {
            var response = new SingleResponse<BaseResponse>()
            {
                Result = new BaseResponse()
            };

            try
            {
                if (IsValidBearerRequest)
                {
                   
                    (int status, string message) = _userRepository.UpdateProfileImage(model.ImageUrl,User_Id);
                    response.Statuscode = status > 0 ? System.Net.HttpStatusCode.OK :
                       status == -2 ? System.Net.HttpStatusCode.Conflict : System.Net.HttpStatusCode.NotFound;
                    response.Message = message;

                }
                else
                {
                    response.ToHttpForbiddenResponse(AppConstants.BEARER_ERRMESSAGE);
                }
            }
            catch (Exception ae)
            {
                response.Message = ae.Message;
                response.Haserror = true;
            }

            return Ok(response);
        }
    }
}
