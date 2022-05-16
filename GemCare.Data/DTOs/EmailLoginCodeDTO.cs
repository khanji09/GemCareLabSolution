using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemCare.Data.DTOs
{
    public class EmailLoginCodeDTO
    {
        public int UserId { get; set; }
        public int EmailCode { get; set; }
    }
}
