using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemCare.Data.DTOs
{
    public class AppointmentDTO
    {
        public int BookingId { get; set; }
        public string CustomerName { get; set; }
        public string ServiceName { get; set; }
        public string TechnicianName { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsCancelled { get; set; }
        public int ReviewPoints { get; set; }
        public string Comments { get; set; }
    }
}
