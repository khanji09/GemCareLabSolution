using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GemCare.API.Contracts.Response
{
    public class BookingImageUploadResponse: BaseResponse
    {
        public string ImagePath { get; set; }
    }
    public class ProfileImageUploadResponse : BaseResponse
    {
        public string ImagePath { get; set; }
    }
}
