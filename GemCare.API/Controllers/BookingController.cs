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
            var response = new SingleResponse<BookingResponse>()
            {
                Result = new BookingResponse()
            };
            try
            {
                if (IsValidBearerRequest)
                {
                    BookingDTO booking = new()
                    {
                        Email = model.Email,
                        Name = model.Name,
                        PostalCode = model.Postalcode,
                        MobileNumber = model.Mobilenumber,
                        Address = model.Address,
                        ImagePath = model.Imagepath,
                        ServiceId = model.Serviceid,
                        UserId = User_Id,
                        WorkDescription = model.Workdescription,
                        RequiredDate = model.Requireddate,
                        AddressNotes = model.Addressnotes
                    };
                    (int status, string message, int bookingid) = _bookingRepository.AddBooking(booking);
                    response.Statuscode = status > 0 ? System.Net.HttpStatusCode.OK :
                       status == -2 ? System.Net.HttpStatusCode.Conflict : System.Net.HttpStatusCode.NotFound;
                    response.Message = message;
                    response.Result.Bookingid = bookingid;
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

        [HttpGet("CompletedBookings")]
        public IActionResult CompletedBookings()
        {
            var response = new SingleResponse<UserCompleteBookingsResponse>()
            {
                Result = new UserCompleteBookingsResponse()
            };
            response.Result.CompletedBookings = new List<UserBookingResponse>();
            try
            {
                if (IsValidBearerRequest)
                {

                    (int status, string message, List<UserBookingDTO> bookings) = _bookingRepository.UserCompletedBookings(User_Id);
                    response.Statuscode = status > 0 ? System.Net.HttpStatusCode.OK :
                       status == -2 ? System.Net.HttpStatusCode.Conflict : System.Net.HttpStatusCode.NotFound;
                    response.Message = message;
                    if (bookings.Count > 0)
                    {
                        response.Result.CompletedBookings = (from b in bookings
                                                             select new UserBookingResponse()
                                                             {
                                                                 Bookingid = b.BookingId,
                                                                 Address = b.Address,
                                                                 Createdon = b.CreatedOn,
                                                                 Customername = b.CustomerName,
                                                                 Email = b.Email,
                                                                 Expecteddate = b.ExpectedDate,
                                                                 Imagepath = b.ImagePath,
                                                                 Mobilenumber = b.MobileNumber,
                                                                 Paidamount = b.PaidAmount,
                                                                 Postalcode = b.PostalCode,
                                                                 Requireddate = b.RequiredDate,
                                                                 Servicename = b.ServiceName,
                                                                 Userid = b.UserId,
                                                                 Workdescription = b.WorkDescription

                                                             }).ToList();
                    }
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

        [HttpGet("UpComingBookings")]
        public IActionResult UpComingBookings()
        {
            var response = new SingleResponse<UserUpCommingBookingsResponse>()
            {
                Result = new UserUpCommingBookingsResponse()
            };
            response.Result.UpComingBookings = new List<UserBookingResponse>();
            try
            {
                if (IsValidBearerRequest)
                {

                    (int status, string message, List<UserBookingDTO> bookings) = _bookingRepository.UserUpComingBookings(User_Id);
                    response.Statuscode = status > 0 ? System.Net.HttpStatusCode.OK :
                       status == -2 ? System.Net.HttpStatusCode.Conflict : System.Net.HttpStatusCode.NotFound;
                    response.Message = message;
                    if (bookings.Count > 0)
                    {
                        response.Result.UpComingBookings = (from b in bookings
                                                            select new UserBookingResponse()
                                                            {
                                                                Bookingid = b.BookingId,
                                                                Address = b.Address,
                                                                Createdon = b.CreatedOn,
                                                                Customername = b.CustomerName,
                                                                Email = b.Email,
                                                                Expecteddate = b.ExpectedDate,
                                                                Imagepath = b.ImagePath,
                                                                Mobilenumber = b.MobileNumber,
                                                                Paidamount = b.PaidAmount,
                                                                Postalcode = b.PostalCode,
                                                                Requireddate = b.RequiredDate,
                                                                Servicename = b.ServiceName,
                                                                Userid = b.UserId,
                                                                Workdescription = b.WorkDescription

                                                            }).ToList();
                    }
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

        [HttpDelete("DeleteBooking/{bookingid}")]
        public IActionResult DeleteBooking(int bookingid)
        {
            BaseResponse response = new BaseResponse();
          
            try
            {
                if (IsValidBearerRequest)
                {

                    (int status, string message) = _bookingRepository.DeleteBooking(bookingid);
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
