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
    public class MultiBookingImageUploadResponse : BaseResponse
    {
        public List<string> ImagesPath { get; set; }
    }
    public class ProfileImageUploadResponse : BaseResponse
    {
        public string ImagePath { get; set; }
    }
    public class BookingVideoUploadResponse : BaseResponse
    {
        public string VideoPath { get; set; }
    }
}
