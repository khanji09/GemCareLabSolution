using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GemCare.API.Contracts.Request
{
    public abstract class BaseGetRequest
    {
        [Required]
        public double Timestamp { get; set; }
    }

    public abstract class BaseApiKeyRequest
    {
        [Required]
        public string Apikey { get; set; }
    }

    public abstract class BaseAuthTokenRequest
    {
        [Required]
        public string Authtoken { get; set; }
    }
}
