using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GemCare.API.Helper
{
    public interface IImageHelper
    {
        Task<string> UploadBookingImage(IFormFile file,int userid);
        
    }
    public class ImageHelper : IImageHelper
    {
        readonly string[] fileExtensions = { ".jpg", ".jpeg", ".png", ".docx", ".pdf" };
        public async Task<string> UploadBookingImage(IFormFile file,int userid)
        {
            string guid = Guid.NewGuid().ToString().Replace("-", "");
            if (file == null) throw new ArgumentNullException("Please select file to upload");
            if (file.Length == 0) throw new ArgumentException("Please select valid file to upload");
            if (!IsValidMediaFile(file)) throw new Exception("Invalid file");
            //
            string fileExt = Path.GetExtension(file.FileName.ToLower());
            string directoryPath = $"Uploads/Images/Booking/{userid}/";
            _ = Directory.CreateDirectory(directoryPath);
            string imagePath, fileName;
            fileName = $"img_{guid}{fileExt}";
            imagePath = $"{directoryPath}/{fileName}";
            using var fileStream = new FileStream(imagePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return imagePath;
        }
      
        private bool IsValidMediaFile(IFormFile fileToUpload)
        {
            var videoExtension = fileToUpload.FileName.Split('.');
            if (videoExtension.Length > 2) throw new Exception("Invalid file");
            // return
            return -1 != Array.IndexOf(fileExtensions, Path.GetExtension(fileToUpload.FileName).ToLowerInvariant());
        }
    }
}
