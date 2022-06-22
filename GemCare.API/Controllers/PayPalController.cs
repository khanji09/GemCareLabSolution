using GemCare.API.Common;
using GemCare.API.Contracts.Request;
using GemCare.API.Contracts.Response;
using GemCare.API.Interfaces;
using GemCare.API.PayPal.Business;
using GemCare.API.PayPal.Business.Request;
using GemCare.API.PayPal.Business.Response;
using GemCare.Data.DTOs;
using GemCare.Data.Interfaces;
using GemCare.Data.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GemCare.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayPalController : BaseApiController
    {

        private readonly ITokenGenerator _tokenGenerator;
        private readonly IPayPalService _payPalService;
        private readonly IBookingRepository _bookingRepository;
        private readonly IPaymentRepository _paymentRepository;
        public PayPalController(ITokenGenerator tokenGenerator,
            IBookingRepository bookingRepository, IPayPalService payPalService,
            IPaymentRepository paymentRepository)
        {
            _tokenGenerator = tokenGenerator;
            _payPalService = payPalService;
            _bookingRepository = bookingRepository;
            _paymentRepository = paymentRepository;
        }
        //[HttpGet("getaccesstoken")]
        //public IActionResult getAccessToken()
        //{
        //    var response = new BaseResponse();
        //    response.Message = PayPal.Business.AccessToken.GetAccessTokenWithBearer();
        //    return Ok(response);
        //}

        [HttpPost("CreateOrder")]
        public IActionResult CreateOrder(PayPalCreateOrderRequest model)
        {
            //bool isvalidrequest = true;
            ISingleResponse<CreateOrderResponse> response = new SingleResponse<CreateOrderResponse>()
            {
                Result = new CreateOrderResponse()
            };
            try
            {
                if (IsValidBearerRequest)
                {
                    (int status, string message, BookingDetailsDTO booking) = _bookingRepository.BookingDetails(model.BookingId);

                    #region order object
                    CreateOrderRequest orderRequest = new CreateOrderRequest()
                    {
                        intent = "CAPTURE",
                        //processing_instruction = "ORDER_COMPLETE_ON_PAYMENT_APPROVAL",
                        processing_instruction = "NO_INSTRUCTION",
                        purchase_units = new[]
                        {
                     new PayPal.Business.Request.Purchase_Units()
                     {
                         items = new PayPal.Business.Request.Item[]
                         {
                           new PayPal.Business.Request.Item()
                           {
                           description=string.IsNullOrEmpty(booking.WorkDescription)?"description":booking.WorkDescription,
                           name=booking.ServiceName,
                            quantity="1",
                            unit_amount=new PayPal.Business.Request.Unit_Amount()
                            {
                                  currency_code=model.Currencycode,
                                  value=model.Amount.ToString()
                            }
                           }
                         },
                          amount=new  PayPal.Business.Request.Amount()
                          {
                               currency_code=model.Currencycode,
                                value=model.Amount.ToString(),
                                 breakdown=new PayPal.Business.Request.Breakdown()
                                 {
                                      item_total=new PayPal.Business.Request.Item_Total()
                                      {
                                           currency_code=model.Currencycode,
                                            value=model.Amount.ToString()
                                      }
                                 }
                          }

                     }
                 },
                        application_context = new Application_Context()
                        {
                            cancel_url = model.Cancelurl,
                            return_url = model.Returnurl,
                        }
                    };
                    #endregion

                    response.Message = "success";
                    response.Result = _payPalService.CreateOrder(orderRequest);
                    if (response.Result != null && string.Equals(response.Result.status.Trim().ToUpper(), "CREATED"))
                    {
                        (int _status, string _message) = _paymentRepository.SavePayPalPaymentInfo(new PayPalPaymentDTO()
                        {
                            BookingId = model.BookingId,
                            PaidAmount = model.Amount,
                            OrderId = response.Result.id,
                            PaypalRequestId = response.Result.PayPalRequestId
                        });
                        response.Statuscode = _status == 1 ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.ExpectationFailed;
                        response.Message = _message;
                    }
                    else
                    {
                        response.Message = $"Order failure - {response.Result.status.ToUpper()} - {response.Result.processing_instruction}";
                        response.Haserror = true;
                        response.Statuscode = System.Net.HttpStatusCode.Forbidden;
                    }
                }
                else
                {
                    response.Message = AppConstants.BEARER_ERRMESSAGE;
                    response.Haserror = true;
                    response.Statuscode = System.Net.HttpStatusCode.NotFound;
                }
            }
            catch (Exception ae)
            {
                response.ToHttpExceptionResponse(ae.Message);
            }
            return Ok(response);
        }

        [HttpPost("CapturePayment")]
        public IActionResult CapturePayment(PayPalCapturePaymentRequest model)
        {
            //bool isvalidrequest = true;
            ISingleResponse<CaptureOrderResponse> response = new SingleResponse<CaptureOrderResponse>()
            {
                Result = new CaptureOrderResponse()
            };
            try
            {
                if (IsValidBearerRequest)
                {
                    response.Message = "success";
                    response.Result = _payPalService.CapturePayment(model);
                    if (response.Result != null && !string.IsNullOrEmpty(response.Result.status.Trim().ToUpper()) &&
                        string.Equals(response.Result.status.Trim().ToUpper(), "COMPLETED"))
                    {
                        (int _status, string _message) = _paymentRepository.UpdatePayPalPaymentInfo(
                             new UpdatePayPalInfoDTO()
                             {
                                 fee = double.Parse(response.Result.purchase_units[0].payments.captures[0].seller_receivable_breakdown.paypal_fee.value),
                                 OrderId = model.Orderid,
                                 PayerId = model.Payerid,
                                 PaypalRequestId = model.PayPalrequestid,
                                 Token = model.Token
                             }
                            );
                        response.Statuscode = _status == 1 ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.ExpectationFailed;
                        response.Message = _message;
                    }
                    else
                    {
                        response.Haserror = true;
                        response.Statuscode = System.Net.HttpStatusCode.NotFound;
                        response.Message = "Payment can't capture";
                    }
                }
                else
                {
                    response.Message = AppConstants.BEARER_ERRMESSAGE;
                    response.Haserror = true;
                    response.Statuscode = System.Net.HttpStatusCode.NotFound;
                }
            }
            catch(Exception ae)
            {
                response.Haserror = true;
                response.Statuscode = System.Net.HttpStatusCode.NotFound;
                response.Message = ae.Message;
            }
            return Ok(response);
        }

        [HttpPost("capturepayment_android")]
        public IActionResult CapturePayment_Android(PayPalCapturePaymentRequest_New request)
        {
            ISingleResponse<CaptureOrderResponse> response = new SingleResponse<CaptureOrderResponse>()
            {
                Result = new CaptureOrderResponse()
            };

            try
            {
                if (IsValidBearerRequest)
                {
                    var result = AccessToken.GetAccessToken();
                    PayPalCapturePaymentRequest model = new()
                    {
                        Orderid = request.Orderid,
                        Token = request.Token,
                        Payerid = request.Payerid,
                        PayPalrequestid = request.PayPalrequestid
                    };
                    response.Message = "success";
                    response.Result = _payPalService.CapturePayment(model);
                    if (response.Result != null && !string.IsNullOrEmpty(response.Result.status.Trim().ToUpper()) &&
                        string.Equals(response.Result.status.Trim().ToUpper(), "COMPLETED"))
                    {
                        (int _status, string _message) = _paymentRepository.InsertUpdatePayPalPaymentInfo(
                             new InsertUpdatePayPalInfoDTO()
                             {
                                 BookingId = request.Bookingid,
                                 PaidAmount = request.Amount,
                                 Fee = double.Parse(response.Result.purchase_units[0].payments.captures[0].seller_receivable_breakdown.paypal_fee.value),
                                 OrderId = request.Orderid,
                                 PayerId = request.Payerid,
                                 PaypalRequestId = request.PayPalrequestid,
                                 Token = request.Token
                             }
                            );
                    }
                    else
                    {
                        response.Haserror = true;
                        response.Statuscode = System.Net.HttpStatusCode.NotFound;
                        response.Message = "Payment can't capture";
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
