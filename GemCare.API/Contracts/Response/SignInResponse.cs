using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GemCare.API.Contracts.Response
{
    public class SignInResponse
    {
        public int Userid { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Authtoken { get; set; }
        public string Refreshtoken { get; set; }
        public string Imagepath { get; set; }
    }
}
