﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GemCare.API.Contracts.Request
{
    public class SignInRequest
    {
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Password { get; set; }

    }
}