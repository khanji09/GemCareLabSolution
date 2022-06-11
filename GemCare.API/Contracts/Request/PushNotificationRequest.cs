using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GemCare.API.Contracts.Request
{
    public class PushNotificationSaveRequest
    {
        //[Required]
        //public int Userid { get; set; }

        [Required]
        public string Pushtoken { get; set; }

        [Required]
        public string Deviceid { get; set; }

        [Required]
        public string Deviceplatform { get; set; }
    }
}
