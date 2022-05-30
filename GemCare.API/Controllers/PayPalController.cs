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
            ISingleResponse<CreateOrderResponse> response = new SingleResponse<CreateOrderResponse>()
            {
                Result = new CreateOrderResponse()
            };
            try
            {
                if (IsValidApiKeyRequest)
                {
                    (int status, string message, BookingDetailsDTO booking) = _bookingRepository.BookingDetails(model.BookingId);

                    #region order object
                    CreateOrderRequest orderRequest = new CreateOrderRequest()
                    {
                        intent = "CAPTURE",
                        processing_instruction = "ORDER_COMPLETE_ON_PAYMENT_APPROVAL",
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
                    }
                    else
                    {
                        response.Message = "Order failure";
                        response.Haserror = true;
                        response.Statuscode = System.Net.HttpStatusCode.Forbidden;
                    }
                }
                else
                {
                    response.Message = AppConstants.APIKEY_ERRMESSAGE;
                    response.Haserror = true;
                    response.Statuscode = System.Net.HttpStatusCode.NotFound;
                }
            }
            catch (Exception ae)
            {
                response.Haserror = true;
                response.Message = ae.Message;
            }
            return Ok(response);
        }

        [HttpPost("CapturePayment")]
        public IActionResult CapturePayment(PayPalCapturePaymentRequest model)
        {
            ISingleResponse<CaptureOrderResponse> response = new SingleResponse<CaptureOrderResponse>()
            {
                Result = new CaptureOrderResponse()
            };
            try
            {
                if (IsValidApiKeyRequest)
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
                    response.Message = AppConstants.APIKEY_ERRMESSAGE;
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
    }
}
