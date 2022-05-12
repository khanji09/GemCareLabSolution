using GemCare.API.Common;
using GemCare.API.Contracts.Response;
using GemCare.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GemCare.API.Controllers
{
    public class SliderController : BaseApiController
    {
        private readonly IGeneralRepository _generalRepository;
        public SliderController(IGeneralRepository generalRepository)
        {
            _generalRepository = generalRepository;
        }

        [HttpGet]
        public IActionResult GetSlider(bool ismobile)
        {
            IListResponse<SliderResponse> response = new ListResponse<SliderResponse>
            {
                Result = new List<SliderResponse>()
            };
            if (IsValidApiKeyRequest)
            {
                var (status, message, images) = _generalRepository.GetSliderImages(ismobile);
                response.Statuscode = 1 == status ? HttpStatusCode.OK : HttpStatusCode.NotFound;
                response.Message = message;
                if(images?.Count > 0)
                {

                }
            }
            else
            {
                response.ToHttpForbiddenResponse(AppConstants.APIKEY_ERRMESSAGE);
            }
            return Ok(response);
        }
    }
}
