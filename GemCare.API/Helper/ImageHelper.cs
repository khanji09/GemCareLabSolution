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
        Task<List<string>> UploadBookingImagesList(List<IFormFile> file, int userid);
        Task<string> UploadProfileImage(IFormFile file, int userid);
        Task<string> UploadValuationImage(IFormFile file, int userid);
        Task<string> UploadValuationVideo(IFormFile file, int userid);

    }
    public class ImageHelper : IImageHelper
    {
        readonly string[] fileExtensions = { ".jpg", ".jpeg", ".png", ".docx", ".pdf" };
        readonly string[] videoExtensions = {".WEBM",".MPG", ".MP2", ".MPEG", ".MPE", ".MPV",".OGG",".MP4", ".M4P", ".M4V",".AVI",".WMV",".MOV", ".QT",".FLV", ".SWF",".AVCHD"};
        public async Task<string> UploadBookingImage(IFormFile file,int userid)
        {
            string guid = Guid.NewGuid().ToString().Replace("-", "");
            if (file == null) throw new ArgumentNullException("Please select file to upload");
            if (file.Length == 0) throw new ArgumentException("Please select valid file to upload");
            if (!IsValidMediaFile(file)) throw new Exception("Invalid file");
            //
            string fileExt = Path.GetExtension(file.FileName.ToLower());
            string directoryPath = $"Uploads/Images/Booking/{userid}";
            _ = Directory.CreateDirectory(directoryPath);
            string imagePath, fileName;
            fileName = $"img_{guid}{fileExt}";
            imagePath = $"{directoryPath}/{fileName}";
            using var fileStream = new FileStream(imagePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return imagePath;
        }

        public async Task<List<string>> UploadBookingImagesList(List<IFormFile> files, int userid)
        {
           
            if (files == null && files.Count<=0) throw new ArgumentNullException("Please select file to upload");
            foreach (IFormFile file in files) {
                if (!IsValidMediaFile(file))
                {
                    throw new Exception("Invalid file");                   
                }
            }
            string guid = "",imagePath="",fileName="";
            string fileExt = "";
            string directoryPath = "";
            List<string> imagesPath = new List<string>();
            foreach (IFormFile file in files)
            {
                 guid = Guid.NewGuid().ToString().Replace("-", "");
                 fileExt = Path.GetExtension(file.FileName.ToLower());
                 directoryPath = $"Uploads/Images/Booking/{userid}";
                _ = Directory.CreateDirectory(directoryPath);
               
                fileName = $"img_{guid}{fileExt}";
                imagePath = $"{directoryPath}/{fileName}";
                using var fileStream = new FileStream(imagePath, FileMode.Create);
                await file.CopyToAsync(fileStream);
                imagesPath.Add( imagePath);
            }
            return imagesPath;
        }

        public async Task<string> UploadProfileImage(IFormFile file, int userid)
        {
            string guid = Guid.NewGuid().ToString().Replace("-", "");
            if (file == null) throw new ArgumentNullException("Please select file to upload");
            if (file.Length == 0) throw new ArgumentException("Please select valid file to upload");
            if (!IsValidMediaFile(file)) throw new Exception("Invalid file");
            //
            string fileExt = Path.GetExtension(file.FileName.ToLower());
            string directoryPath = $"Uploads/Images/Profile/{userid}";
            var directoryInfo = Directory.CreateDirectory(directoryPath);
            string imagePath, fileName;
            fileName = $"img_{guid}{fileExt}";
            imagePath = $"{directoryPath}/{fileName}";
            using var fileStream = new FileStream(imagePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return imagePath;
        }

        public async Task<string> UploadValuationImage(IFormFile file, int userid)
        {
            string guid = Guid.NewGuid().ToString().Replace("-", "");
            if (file == null) throw new ArgumentNullException("Please select file to upload");
            if (file.Length == 0) throw new ArgumentException("Please select valid file to upload");
            if (!IsValidMediaFile(file)) throw new Exception("Invalid file");
            //
            string fileExt = Path.GetExtension(file.FileName.ToLower());
            string directoryPath = $"Uploads/Images/Valuation/{userid}";
            var directoryInfo = Directory.CreateDirectory(directoryPath);
            string imagePath, fileName;
            fileName = $"img_{guid}{fileExt}";
            imagePath = $"{directoryPath}/{fileName}";
            using var fileStream = new FileStream(imagePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return imagePath;
        }

        public async Task<string> UploadValuationVideo(IFormFile file, int userid)
        {
            string guid = Guid.NewGuid().ToString().Replace("-", "");
            if (file == null) throw new ArgumentNullException("Please select file to upload");
            if (file.Length == 0) throw new ArgumentException("Please select valid file to upload");
            if (!IsValidVideoFile(file)) throw new Exception("Invalid file");
            //
            string fileExt = Path.GetExtension(file.FileName.ToLower());
            string directoryPath = $"Uploads/Videos/Valuation/{userid}";
            var directoryInfo = Directory.CreateDirectory(directoryPath);
            string imagePath, fileName;
            fileName = $"vid_{guid}{fileExt}";
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

        private bool IsValidVideoFile(IFormFile fileToUpload)
        {
            var videoExtension = Path.GetExtension(fileToUpload.FileName).ToUpper().Trim();
            if (videoExtensions.Contains(videoExtension))
                return true;
            else
                return false;
        }
    }
}
