using GemCare.API.Common;
using GemCare.API.Contracts.Request;
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
    public class AdminController : BaseApiController
    {
        private readonly IAppointmentRepository _appointments;
        private readonly IDashboardRepository _dashboardRepository;
        public AdminController(IAppointmentRepository appointments, IDashboardRepository dashboardRepository)
        {
            _appointments = appointments;
            _dashboardRepository = dashboardRepository;
        }

        [HttpGet("jobs")]
        public IActionResult GetBookings(int type, int pagenumber, int pagesize)
        {
            //
            IPagedResponse<AppointmentResponse> response = new PagedResponse<AppointmentResponse>();
            if (IsValidBearerRequest)
            {
                try
                {
                    var (status, message, appointments, totalPages, totalRecords) = _appointments.GetAppointments(type, pagenumber, pagesize);
                    response.Statuscode = 1 == status ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.NotFound;
                    response.Message = message;
                    response.Totalpages = totalPages;
                    response.Totalrecords = totalRecords;
                    if(appointments?.Count > 0)
                    {
                        response.Result = new List<AppointmentResponse>(pagesize);
                        appointments.ForEach(appointment =>
                        {
                            response.Result.Add(new AppointmentResponse
                            {
                                Id = appointment.BookingId,
                                Customername = appointment.CustomerName,
                                Servicename = appointment.ServiceName,
                                Technicianname = appointment.TechnicianName,
                                Iscompleted = appointment.IsCompleted,
                                Iscancelled = appointment.IsCancelled
                            });
                        });
                    }
                }
                catch(Exception ex)
                {
                    response.ToHttpExceptionResponse(ex.Message);
                }
            }
            else
            {
                response.ToHttpForbiddenResponse(AppConstants.BEARER_ERRMESSAGE);
            }
            return Ok(response);
        }

        [HttpGet("technicians")]
        public IActionResult GetTechnicians()
        {
            IListResponse<TechnicianResponse> response = new ListResponse<TechnicianResponse>();
            if (IsValidBearerRequest)
            {
                try
                {
                    var (status, message, technicians) = _dashboardRepository.GetTechnicians();
                    response.Statuscode = 1 == status ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.NotFound;
                    response.Message = message;
                    if (technicians?.Count > 0)
                    {
                        response.Result = new List<TechnicianResponse>();
                        technicians.ForEach(technician =>
                        {
                            response.Result.Add(new TechnicianResponse
                            {
                                Id = technician.Id,
                                Firstname = technician.FirstName,
                                Lastname = technician.LastName,
                                Email = technician.Email,
                                Mobile = technician.Mobile
                            });
                        });
                    }
                }
                catch (Exception ex)
                {
                    response.ToHttpExceptionResponse(ex.Message);
                }
            }
            else
            {
                response.ToHttpForbiddenResponse(AppConstants.BEARER_ERRMESSAGE);
            }
            return Ok(response);
        }

        [HttpPut("assignjob")]
        public IActionResult AssignJob([FromBody] AssignJobRequest request)
        {
            IBaseResponse response = new BaseResponse();
            if (IsValidBearerRequest)
            {
                var (status, message) = _appointments.AssignJobToTechnician(request.Bookingid, request.Technicianid);
                response.Statuscode = 1 == status ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.NotFound;
                response.Message = message;
            }
            else
            {
                response.ToHttpForbiddenResponse(AppConstants.BEARER_ERRMESSAGE);
            }
            return Ok(response);
        }

        [HttpGet("bookingreviews")]
        public IActionResult GetBookingRatings(int pagenumber, int pagesize)
        {
            //
            IPagedResponse<BookingReviewResponse> response = new PagedResponse<BookingReviewResponse>();
            if (IsValidBearerRequest)
            {
                try
                {
                    var (status, message, appointments, totalPages, totalRecords) = _appointments.GetBookingReviews(pagenumber, pagesize);
                    response.Statuscode = 1 == status ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.NotFound;
                    response.Message = message;
                    response.Totalpages = totalPages;
                    response.Totalrecords = totalRecords;
                    if (appointments?.Count > 0)
                    {
                        response.Result = new List<BookingReviewResponse>(pagesize);
                        appointments.ForEach(appointment =>
                        {
                            response.Result.Add(new BookingReviewResponse
                            {
                                Id = appointment.BookingId,
                                Customername = appointment.CustomerName,
                                Servicename = appointment.ServiceName,
                                Technicianname = appointment.TechnicianName,
                                Points = appointment.ReviewPoints,
                                Comments = appointment.Comments
                            });
                        });
                    }
                }
                catch (Exception ex)
                {
                    response.ToHttpExceptionResponse(ex.Message);
                }
            }
            else
            {
                response.ToHttpForbiddenResponse(AppConstants.BEARER_ERRMESSAGE);
            }
            return Ok(response);
        }

        [HttpPost("technician")]
        public IActionResult CreateTechnician(TechnicianCreateRequest request)
        {
            IBaseResponse response = new BaseResponse();
            if (IsValidBearerRequest)
            {
                try
                {
                    UserBasicInfo basicInfo = new()
                    {
                        FirstName = request.Firstname,
                        LastName = request.Lastname,
                        Email = request.Email,
                        Password = request.Password,
                        Mobile = request.Mobile
                    };
                    var (status, message) = _dashboardRepository.TechnicianRegistration(basicInfo);
                    response.Statuscode = 1 == status ? System.Net.HttpStatusCode.Created : System.Net.HttpStatusCode.ExpectationFailed;
                    response.Message = message;
                }
                catch(Exception ex)
                {
                    response.ToHttpExceptionResponse(ex.Message);
                }
            }
            else
            {
                response.ToHttpForbiddenResponse(AppConstants.BEARER_ERRMESSAGE);
            }
            return Ok(response);
        }
    }
}
