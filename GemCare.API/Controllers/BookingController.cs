using GemCare.API.Common;
using GemCare.API.Contracts.Request;
using GemCare.API.Contracts.Response;
using GemCare.API.Interfaces;
using GemCare.API.Services;
using GemCare.Data.DTOs;
using GemCare.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GemCare.API.Controllers
{
    public class BookingController : BaseApiController
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IPushService _pushService;
        private readonly IAddressService _addressService;
        public BookingController(IBookingRepository bookingRepository, IPushService pushService,
            IAddressService addressService)
        {
            _bookingRepository = bookingRepository;
            _pushService = pushService;
            _addressService = addressService;
        }
        [HttpPost("AddBooking")]
        public IActionResult AddBooking([FromBody] BookingRequest model)
        {
            var response = new SingleResponse<BookingResponse>()
            {
                Result = new BookingResponse()
            };
            try
            {
                if (IsValidBearerRequest)
                {
                    var validatePostalCode = Task.Run(() => _addressService.IsValidPostalCode(model.Postalcode));
                    if (validatePostalCode.Result)
                    {
                        BookingDTO booking = new()
                        {
                            Email = model.Email,
                            Name = model.Name,
                            PostalCode = model.Postalcode,
                            MobileNumber = model.Mobilenumber,
                            Address = model.Address,
                            ImagesPath = model.Imagespath,
                            ServiceId = model.Serviceid,
                            UserId = User_Id,
                            WorkDescription = model.Workdescription,
                            RequiredDate = model.Requireddate,
                            AddressNotes = model.Addressnotes,
                             VideoPath=model.Videopath
                        };
                        (int status, string message, int bookingid) = _bookingRepository.AddBooking(booking);
                        response.Statuscode = status > 0 ? System.Net.HttpStatusCode.OK :
                           status == -2 ? System.Net.HttpStatusCode.Conflict : System.Net.HttpStatusCode.NotFound;
                        response.Message = message;
                        response.Result.Bookingid = bookingid;
                        //if (status > 0)
                        //{
                        //    var result = _bookingRepository.GetAdminDeviceInfoForBookingNotification(bookingid);
                        //    if (result.deviceInfo != null)
                        //    {
                        //        var result1 = _pushService.SendBookingNotificationToAdmin(result.deviceInfo.PushTitle,
                        //            result.deviceInfo.PushBody, result.deviceInfo.PushToken, result.deviceInfo.DevicePlatform);
                        //    }
                        //}
                    }
                    else
                    {
                        response.Statuscode = System.Net.HttpStatusCode.BadRequest;
                        response.Message = "Invalid postalcode";
                        response.Haserror = true;
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
                                                                 Imagespath = b.ImagePath.Split("|").ToList(),
                                                                 Videopath=b.VideoPath,
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
                                                                Imagespath = b.ImagePath.Split("|").ToList(),
                                                                Videopath=b.VideoPath,
                                                                Mobilenumber = b.MobileNumber,
                                                                Paidamount = b.PaidAmount,
                                                                Postalcode = b.PostalCode,
                                                                Requireddate = b.RequiredDate,
                                                                Servicename = b.ServiceName,
                                                                Userid = b.UserId,
                                                                Workdescription = b.WorkDescription,
                                                                 

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
