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

    public class ValuationRequestDTO
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public int TechnicianId { get; set; }
        public string TechFirstName { get; set; }
        public string TechLastName { get; set; }
        public string ItemDescription { get; set; }
        public string ImageUrl { get; set; }
        public string VideoUrl { get; set; }
        public int Quotation { get; set; }
        public string ServiceName { get; set; }
    }
}
