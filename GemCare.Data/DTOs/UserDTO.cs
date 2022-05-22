using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemCare.Data.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }      
        public string Email { get; set; }
        public string Mobile { get; set; }
        public DateTime DOB { get; set; }
        public bool IsAccountConfirmed { get; set; }
        public int UserTypeId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class UserProfileDTO:UserDTO
    {
        public string FullName { get; set; }
        public string ImagePath { get; set; }
        public string Gender { get; set; }
    }
}
