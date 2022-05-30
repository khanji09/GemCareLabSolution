using Microsoft.AspNetCore.Http;
using System;

namespace GemCare.API.Contracts.Request
{
    public class UserProfileUpdateRequest
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string Gender { get; set; }
        public DateTime DOB { get; set; }
    }
    public class UserProfileImageRequest : BaseAuthTokenRequest
    {
        public int Userid { get; set; }
        public IFormFile Profileimage { get; set; }
    }

    public class UserProfileUpdateImageRequest
    {
        public int Userid { get; set; }
        public string ImageUrl { get; set; }
    }
}
