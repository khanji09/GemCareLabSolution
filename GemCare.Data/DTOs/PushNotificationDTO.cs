using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemCare.Data.DTOs
{
    public class PushDTO
    {
        public string PushTitle { get; set; }
        public string PushBody { get; set; }
        public string PushToken { get; set; }
        public string DevicePlatform { get; set; }
    }
}
