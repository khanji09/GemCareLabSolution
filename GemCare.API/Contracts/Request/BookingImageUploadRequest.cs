using GemCare.API.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GemCare.API.Contracts.Request
{
    public class BookingImageUploadRequest : BaseAuthTokenRequest
    {
        [GreaterThanZero(ErrorMessage ="{0} must be greater than zero")]
        public int  Userid { get; set; }

        public IFormFile bookingimage { get; set; } 
    }
}
