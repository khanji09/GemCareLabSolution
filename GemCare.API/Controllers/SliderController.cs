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
        private readonly IImageSliderRepository _ImageSliderRepository;
        public SliderController(IImageSliderRepository imageSliderRepository)
        {
            _ImageSliderRepository = imageSliderRepository;
        }

        [HttpGet("GetSlider/{ismobile}")]
        public IActionResult GetSlider(bool ismobile)
        {
            IListResponse<SliderResponse> response = new ListResponse<SliderResponse>
            {
                Result = new List<SliderResponse>()
            };
            if (IsValidApiKeyRequest)
            {
                var (status, message, Sliderimages) = _ImageSliderRepository.GetSliderImages(ismobile);
                response.Statuscode = 1 == status ? HttpStatusCode.OK : HttpStatusCode.NotFound;
                response.Message = message;
                if (Sliderimages?.Count > 0)
                {
                    response.Result = (from img in Sliderimages
                                       select new SliderResponse()
                                       {
                                           Imageurl = img.ImageUrl,
                                           Shortdescription =img.ShortDescription
                                       }).ToList();
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
