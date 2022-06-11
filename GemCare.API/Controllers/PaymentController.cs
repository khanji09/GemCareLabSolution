using GemCare.API.Common;
using GemCare.API.Contracts.Request;
using GemCare.API.Contracts.Response;
using GemCare.Data.DTOs;
using GemCare.Data.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GemCare.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : BaseApiController
    {
        private readonly IPaymentRepository _paymentRepository;
        public PaymentController(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        [HttpPost("createpaymentmethodid")]
        public IActionResult CreatePaymentMethodId([FromBody] PaymentMethodCreateRequest request)
        {
            //bool isvalidrequest = true;
            ISingleResponse<PaymentMethodCreateResponse> response = new SingleResponse<PaymentMethodCreateResponse>
            {
                Result = new()
            };
            if (IsValidBearerRequest)
            {
                try
                {
                    var paymentMethodCreateOptions = new PaymentMethodCreateOptions
                    {
                        Type = "card",
                        Card = new PaymentMethodCardOptions
                        {
                            Token = request.Stripetoken
                        }
                    };
                    var paymentMethodService = new PaymentMethodService();
                    // stripe methods for adding customer and payment method.
                    PaymentMethod payMethod = paymentMethodService.Create(paymentMethodCreateOptions);
                    response.Result.Paymentmethodid = payMethod.Id;
                    response.Statuscode = System.Net.HttpStatusCode.OK;
                    response.Message = AppConstants.SUCCESS_MESSAGE;
                }
                catch(StripeException se)
                {
                    response.ToHttpExceptionResponse(se.Message);
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

        [HttpPost("capturepayment")]
        public IActionResult CapturePayment([FromBody] CapturePaymentRequest request)
        {
            ISingleResponse<CapturePaymentResponse> response = new SingleResponse<CapturePaymentResponse>
            {
                Result = new()
            };
            var paymentIntentService = new PaymentIntentService();
            if (IsValidBearerRequest)
            {
                try
                {
                    var paymentIntentCreateOptions = new PaymentIntentCreateOptions
                    {
                        Amount = Convert.ToInt64(request.Amount * 100),
                        Currency = request.Currency,
                        PaymentMethodTypes = new List<string> { "card" },
                        CaptureMethod = "manual",
                        Metadata = new Dictionary<string, string>() { { "order_id", request.Bookingid.ToString() },
                        { "booking_by", request.Useremail } },
                        PaymentMethod = request.Paymentmethodid,                      
                        StatementDescriptor = "GemCare Booking",
                        ReceiptEmail = request.Useremail
                    };
                    var intentCreate = paymentIntentService.Create(paymentIntentCreateOptions);
                    if (intentCreate.Status.ToLower().Equals("requires_confirmation"))
                    {
                        var options = new PaymentIntentConfirmOptions
                        {
                            PaymentMethod = request.Paymentmethodid
                        };
                        var intentConfirmed = paymentIntentService.Confirm(intentCreate.Id, options);
                        //
                        var paymentIntentCaptureOptions = new PaymentIntentCaptureOptions
                        {
                            AmountToCapture = Convert.ToInt64(request.Amount * 100)
                        };
                        var intentCapture = paymentIntentService.Capture(intentCreate.Id, paymentIntentCaptureOptions);
                        if (intentCapture.Status == "succeeded")
                        {
                            response.Statuscode = System.Net.HttpStatusCode.OK;
                            response.Message = "Success";
                            response.Result.Transactionid = intentCreate.Id;
                            response.Result.Chargeid = intentCapture.Charges.Select(x => x.Id).FirstOrDefault();
                        }
                        else
                        {
                            response.Statuscode = intentCapture.StripeResponse.StatusCode;
                            response.Message = "Stripe: Payment capture not succeeded";
                        }
                    }
                    else
                    {
                        response.Statuscode = intentCreate.StripeResponse.StatusCode;
                        response.Message = $"Stripe: {intentCreate.Status}";
                    }
                }
                catch (StripeException ex)
                {
                    response.ToHttpExceptionResponse($"Stripe Exception : {ex.Message}");
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

        [HttpPost("savepayment")]
        public IActionResult SavePayment([FromBody] SavePaymentRequest request)
        {
            IBaseResponse response = new BaseResponse();
            try
            {
                PaymentDTO payment = new()
                {
                    BookingId = request.Bookingid,
                    CustomerId = request.Customerid ?? null,
                    PaymentMethodId = request.Paymentmethodid,
                    TransactionId = request.Transactionid,
                    ChargeId = request.Chargeid,
                    Amount = request.Amount
                };
                var (status, message) = _paymentRepository.SaveBookingPaymentInfo(payment);
                response.Statuscode = status >= 0 ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.Conflict;
                response.Message = message;
            }
            catch (Exception ex)
            {
                response.ToHttpExceptionResponse(ex.Message);
            }
            return Ok(response);
        }
    }
}
