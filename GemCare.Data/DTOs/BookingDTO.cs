using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemCare.Data.DTOs
{
    public class BookingDTO
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public int UserId { get; set; }
        public int TechnicianId { get; set; }
        public int AddressId { get; set; }
        public string WorkDescription { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime ExpectedDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public bool IsCompleted { get; set; } = false;
        public bool IsCancelled { get; set; } = false;
        public string CancellationReason { get; set; }
        public string ImagePath { get; set; }
    }
}
