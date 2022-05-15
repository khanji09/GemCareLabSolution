using GemCare.API.Contracts.Request;
using GemCare.API.Contracts.Response;
using GemCare.API.Interfaces;
using GemCare.Data.DTOs;
using GemCare.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace GemCare.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : BaseApiController
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ITokenGenerator _tokenGenerator;
        public BookingController(IBookingRepository bookingRepository, ITokenGenerator tokenGenerator)
        {
            _bookingRepository = bookingRepository;
            _tokenGenerator = tokenGenerator;
        }
        [HttpPost("AddBooking")]
        public IActionResult AddBooking(BookingRequest model)
        {
            var response = new SingleResponse<BaseResponse>();
            try
            {
                if (IsValidBearerRequest)
                {
                    BookingDTO booking = new BookingDTO()
                    {
                        AddressId = model.Addressid,
                        ImagePath = model.Imagepath,
                        ServiceId = model.Serviceid,
                        UserId = User_Id,
                        WorkDescription = model.Workdescription,
                        RequiredDate = model.Requireddate
                    };
                    (int status, string message, int bookingid) = _bookingRepository.AddBooking(booking);
                    response.Statuscode = status > 0 ? System.Net.HttpStatusCode.OK :
                       status == -2 ? System.Net.HttpStatusCode.Conflict : System.Net.HttpStatusCode.NotFound;
                    response.Message = message;
                }
                else
                {
                    response.Message = "Invalid Authtoken";
                    response.Haserror = true;
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
