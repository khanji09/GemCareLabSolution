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
    public class ValuationController : BaseApiController
    {
        private readonly IValuationRepository _valuationRepository;
        private readonly ITokenGenerator _tokenGenerator;
        public ValuationController(IValuationRepository valuationRepository, ITokenGenerator tokenGenerator)
        {
            _valuationRepository = valuationRepository;
            _tokenGenerator = tokenGenerator;
        }

        [HttpPost("AddValuation")]
        public IActionResult AddValuation(ValuationRequest model)
        {
            var response = new SingleResponse<ValuationResponse>()
            {
                Result = new ValuationResponse()
            };
            try
            {
                if (IsValidBearerRequest)
                {
                    ValuationDTO valuation = new ValuationDTO()
                    {
                        CreatedOn = DateTime.Now,
                        CustomerId = model.Customerid,
                        ImageUrl = model.Imagepath,
                        ItemDescription = model.Itemdescription,
                        Quotation = model.Quotation,
                        ServiceId = model.ServiceId,
                        TechnicianId = model.Technicianid,
                        VideoUrl = model.Videopath,
                        UpdatedOn = DateTime.Now

                    };
                    (int status, string message, int valuationid) = _valuationRepository.AddValuation(valuation);
                    response.Statuscode = status > 0 ? System.Net.HttpStatusCode.OK :
                       status == -2 ? System.Net.HttpStatusCode.Conflict : System.Net.HttpStatusCode.NotFound;
                    response.Message = message;
                    response.Result.valuationid = valuationid;
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

        [HttpGet("GetValuationsForAdmin")]
        public IActionResult GetValuationsForAdmin()
        {
            var response = new SingleResponse<AdminValuationListResponse>()
            {
                Result = new AdminValuationListResponse()
            };
            response.Result.ValuationsList = new List<AdminValuationResponse>();
            try
            {
                if (IsValidBearerRequest)
                {

                    (int status, string message, List<AdminValuationDTO> valuations) = _valuationRepository.GetValuationsAdmin();
                    response.Statuscode = status > 0 ? System.Net.HttpStatusCode.OK :
                       status == -2 ? System.Net.HttpStatusCode.Conflict : System.Net.HttpStatusCode.NotFound;
                    response.Message = message;
                    if (valuations.Count > 0)
                    {
                        response.Result.ValuationsList = (from v in valuations
                                                          select new AdminValuationResponse()
                                                          {
                                                              Customerfirstname = v.CustomerFirstName,
                                                              Customerid = v.CustomerId,
                                                              Customerlastname = v.CustomerLastName,
                                                              Id = v.Id,
                                                              Imageurl = v.ImageUrl,
                                                              Itemdescription = v.ItemDescription,
                                                              Quotation = v.Quotation,
                                                              Servicename = v.ServiceName,
                                                              Techfirstname = v.TechFirstName,
                                                              Techlastname = v.TechLastName,
                                                              Technicianid = v.TechnicianId,
                                                              Videourl = v.VideoUrl

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

        [HttpPut("assign")]
        public IActionResult AssignValuation(AssignValuationRequest request)
        {
            IBaseResponse response = new BaseResponse();
            if (IsValidBearerRequest)
            {
                try
                {
                    var (status, message) = _valuationRepository.AssignValuationRequest(request.Id, request.Technicianid);
                    response.Statuscode = status == 1 ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.ExpectationFailed;
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

        [HttpGet("GetValuations/{istechnician}")]
        public IActionResult GetValuations(bool istechnician)
        {
            var response = new SingleResponse<AdminValuationListResponse>()
            {
                Result = new AdminValuationListResponse()
            };
            response.Result.ValuationsList = new List<AdminValuationResponse>();
            try
            {
                if (IsValidBearerRequest)
                {

                    (int status, string message, List<AdminValuationDTO> valuations) = _valuationRepository.GetValuationsRequests(User_Id,istechnician);
                    response.Statuscode = status > 0 ? System.Net.HttpStatusCode.OK :
                       status == -2 ? System.Net.HttpStatusCode.Conflict : System.Net.HttpStatusCode.NotFound;
                    response.Message = message;
                    if (valuations.Count > 0)
                    {
                        response.Result.ValuationsList = (from v in valuations
                                                          select new AdminValuationResponse()
                                                          {
                                                              Customerfirstname = v.CustomerFirstName,
                                                              Customerid = v.CustomerId,
                                                              Customerlastname = v.CustomerLastName,
                                                              Id = v.Id,
                                                              Imageurl = v.ImageUrl,
                                                              Itemdescription = v.ItemDescription,
                                                              Quotation = v.Quotation,
                                                              Servicename = v.ServiceName,
                                                              Techfirstname = v.TechFirstName,
                                                              Techlastname = v.TechLastName,
                                                              Technicianid = v.TechnicianId,
                                                              Videourl = v.VideoUrl

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
    }
}
