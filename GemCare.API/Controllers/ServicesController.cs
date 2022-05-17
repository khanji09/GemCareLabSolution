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
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : BaseApiController
    {
        private readonly IServiceRepository _serviceRepository;
        public ServicesController(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }
        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            ListResponse<AllServicesResponse> response = new ListResponse<AllServicesResponse>()
            {
                Result = new List<AllServicesResponse>()
            };
            if (IsValidApiKeyRequest)
            {
                (int status, string message, List<ServiceDTO> services) = _serviceRepository.GetAllServices();
                response.Statuscode = status > 0 ? System.Net.HttpStatusCode.OK :
                      status == -2 ? System.Net.HttpStatusCode.Conflict : System.Net.HttpStatusCode.NotFound;
                response.Message = message;
                if (status > 0)
                {
                    response.Result = (from service in services
                                       select new AllServicesResponse()
                                       {                                          
                                           Id = service.Id,
                                           ImageUrl = service.ImageUrl,
                                           Name = service.Name,
                                           Price = service.Price                                           
                                       }).ToList();
                }
            }
            else
            {
                response.Message = AppConstants.APIKEY_ERRMESSAGE;
                response.Haserror = true;
                response.Statuscode = System.Net.HttpStatusCode.NotFound;
            }
            return Ok(response);
        }

        [HttpGet("detail/{serviceid}")]
        public IActionResult ServiceDetail(int serviceid)
        {
            ISingleResponse<ServiceResponse> response = new SingleResponse<ServiceResponse>()
            {
                Result = new ServiceResponse()
            };
            if (IsValidApiKeyRequest)
            {
                (int status, string message, ServiceDTO service) = _serviceRepository.GetServiceDetail(serviceid);
                response.Statuscode = status > 0 ? System.Net.HttpStatusCode.OK :
                      status == -2 ? System.Net.HttpStatusCode.Conflict : System.Net.HttpStatusCode.NotFound;
                response.Message = message;
                if (status > 0)
                {
                    response.Result = new ServiceResponse()
                    {
                        Description = service.Description,
                        Id = service.Id,
                        ImageUrl = service.ImageUrl,
                        Name = service.Name,
                        Price = service.Price,
                        ShortDescription = service.ShortDescription
                    };
                }
            }
            else
            {
                response.Message = AppConstants.APIKEY_ERRMESSAGE;
                response.Haserror = true;
                response.Statuscode = System.Net.HttpStatusCode.NotFound;
            }
            return Ok(response);
        }
    }
}
