using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemCare.Data.DTOs
{
    public class AppUser : BaseEntity
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserRole { get; set; }
        public DateTime DOB { get; set; }
        public string Mobile { get; set; }
        public string ImagePath { get; set; }
        public string TutoringMode { get; set; }
        public string TutorType { get; set; }
        public string TutorTypeDetail { get; set; }
        public int MobileConfirmationCode { get; set; }
        public bool IsAccountConfirmed { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
