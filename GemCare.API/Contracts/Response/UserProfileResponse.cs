using System;

namespace GemCare.API.Contracts.Response
{
    public class UserProfileResponse
    {
      
        public int Id { get; set; }      
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime Dateofbirth { get; set; }
        public string Gender { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Imagepath { get; set; }        

    }
}
