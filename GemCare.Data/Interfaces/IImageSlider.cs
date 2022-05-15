using GemCare.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemCare.Data.Interfaces
{
    public interface IImageSliderRepository
    {
        (int status, string message, List<ImageSliderDTO> sliderImages) GetSliderImages(bool isMobile);
    }
}
