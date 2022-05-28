using GemCare.API.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GemCare.API.Contracts.Request
{
    public class CustomerRegisterRequest //: BaseApiKeyRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        //[RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,16}$", ErrorMessage = AppConstants.PASSWORD_COMPLEXITY_TEXT)]
        public string Password { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "First Name : Use letters only please")]
        public string Firstname { get; set; }

        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Last Name : Use letters only please")]
        public string Lastname { get; set; }

        [Required]
        public string Mobile { get; set; }
    }

    public class TechnicianCreateRequest //: BaseApiKeyRequest
    {
        [Required(ErrorMessage = "{0} is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,16}$", ErrorMessage = AppConstants.PASSWORD_COMPLEXITY_TEXT)]
        public string Password { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "First Name : Use letters only please")]
        public string Firstname { get; set; }

        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Last Name : Use letters only please")]
        public string Lastname { get; set; }

        [Required]
        public string Mobile { get; set; }
    }
}
