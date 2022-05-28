using GemCare.API.Common;
using GemCare.API.Contracts.Request;
using GemCare.API.Contracts.Response;
using GemCare.API.Interfaces;
using GemCare.API.PayPal.Business;
using GemCare.API.PayPal.Business.Request;
using GemCare.API.PayPal.Business.Response;
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
    public class PayPalController : BaseApiController
    {

        private readonly ITokenGenerator _tokenGenerator;
        private readonly IPayPalService _payPalService;
        public PayPalController(ITokenGenerator tokenGenerator, IPayPalService payPalService)
        {
            _tokenGenerator = tokenGenerator;
            _payPalService=payPalService;
        }
        [HttpGet("getaccesstoken")]
        public IActionResult getAccessToken()
        {
            var response = new BaseResponse();
            response.Message = PayPal.Business.AccessToken.GetAccessTokenWithBearer();
            return Ok(response);
        }

        [HttpPost("CreateOrder")]
        public IActionResult CreateOrder(PayPalCreateOrderRequest model)
        {
            ISingleResponse<CreateOrderResponse> response = new SingleResponse<CreateOrderResponse>()
            {
                Result = new CreateOrderResponse()
            };
            CreateOrderRequest orderRequest = new CreateOrderRequest()
            {
                intent = "CAPTURE",
                purchase_units = new[]
                 {
                     new PayPal.Business.Request.Purchase_Units()
                     {
                         items = new PayPal.Business.Request.Item[]
                         {
                           new PayPal.Business.Request.Item()
                           {
                            description="gem care lab",
                            name=model.Servicename,
                            quantity="1",
                            unit_amount=new PayPal.Business.Request.Unit_Amount()
                            {
                                  currency_code="USD",
                                  value=model.Amount.ToString()
                            }
                           }
                         },
                          amount=new  PayPal.Business.Request.Amount()
                          {
                               currency_code="USD",
                                value=model.Amount.ToString(),
                                 breakdown=new PayPal.Business.Request.Breakdown()
                                 {
                                      item_total=new PayPal.Business.Request.Item_Total()
                                      {
                                           currency_code="USD",
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

            response.Message = "success";
            response.Result = _payPalService.CreateOrder(orderRequest);
            return Ok(response);
        }
    }
}
