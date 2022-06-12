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
    public class BookingReviewController : BaseApiController
    {
        private readonly IBookingReviewRepository _bookingReviewRepository;
        private readonly ITokenGenerator _tokenGenerator;
        public BookingReviewController(IBookingReviewRepository bookingReviewRepository, ITokenGenerator tokenGenerator)
        {
            _bookingReviewRepository = bookingReviewRepository;
            _tokenGenerator = tokenGenerator;
        }

        [HttpGet("GetBookingReviews/{bookingid}")]
        public IActionResult GetBookingReviews(int bookingid)
        {
            var response = new SingleResponse<CustomerBookingReviewResponseRoot>()
            {
                Result = new CustomerBookingReviewResponseRoot()
            };
            response.Result.BookingReviews = new List<CustomerBookingReviewResponse>();
            try
            {
                if (IsValidBearerRequest)
                {

                    (int status, string message, List<BookingReviewDTO> reviews) = _bookingReviewRepository.GetBookingReview(bookingid);
                    response.Statuscode = status > 0 ? System.Net.HttpStatusCode.OK :
                       status == -2 ? System.Net.HttpStatusCode.Conflict : System.Net.HttpStatusCode.NotFound;
                    response.Message = message;
                    if (reviews.Count > 0)
                    {
                        response.Result.BookingReviews = (from v in reviews
                                                          select new CustomerBookingReviewResponse()
                                                          {
                                                              Bookingid = v.BookingId,
                                                              Comments = v.Comments,
                                                              Id = v.Id,
                                                              Isread = v.IsRead,
                                                              Reviewpoints = v.ReviewPoints

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


        [HttpPut("UpdateReview")]
        public IActionResult UpdateReview(UpdateBookingReviewRequest model)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                if (IsValidBearerRequest)
                {
                    BookingReviewDTO _dto = new BookingReviewDTO()
                    {
                        Comments = model.Comments,
                        ReviewPoints = model.Reviewpoints,
                        Id = model.Id
                    };
                    (int status, string message) = _bookingReviewRepository.UpdateBookingReview(_dto);
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
