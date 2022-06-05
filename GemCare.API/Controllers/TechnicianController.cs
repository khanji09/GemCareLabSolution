using GemCare.API.Common;
using GemCare.API.Contracts.Response;
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
    public class TechnicianController : BaseApiController
    {
        private readonly IBookingRepository _bookingRepository;
        public TechnicianController(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        [HttpGet("pendingjobs")]
        public IActionResult GetPendingJobs()
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

                    (int status, string message, List<UserBookingDTO> bookings) = _bookingRepository
                        .GetTechnicianUpcomingBookings(User_Id);
                    response.Statuscode = status > 0 ? System.Net.HttpStatusCode.OK 
                        : System.Net.HttpStatusCode.NotFound;
                    response.Message = message;
                    if (bookings?.Count > 0)
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
                response.ToHttpExceptionResponse(ae.Message);
            }

            return Ok(response);
        }

        [HttpGet("completedjobs")]
        public IActionResult GetCompletedJobs()
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

                    (int status, string message, List<UserBookingDTO> bookings) = _bookingRepository
                        .GetTechnicianCompletedBookings(User_Id);
                    response.Statuscode = status > 0 ? System.Net.HttpStatusCode.OK 
                        : System.Net.HttpStatusCode.NotFound;
                    response.Message = message;
                    if (bookings?.Count > 0)
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
                response.ToHttpExceptionResponse(ae.Message);
            }

            return Ok(response);
        }
    }
}
