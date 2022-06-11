using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GemCare.API.Contracts.Request
{
    public class SignInRequest
    {
        [Required]
        public string Email { get; set; }
        //[Required]
        //public string Mobile { get; set; }

        [Required]
        public string Password { get; set; }

    }

    public class AdminSignInRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
