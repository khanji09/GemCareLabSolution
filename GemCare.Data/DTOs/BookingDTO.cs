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
        public string WorkDescription { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime ExpectedDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public bool IsCompleted { get; set; } = false;
        public bool IsCancelled { get; set; } = false;
        public string CancellationReason { get; set; }
        public List<string> ImagesPath { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PostalCode { get; set; }
        public string MobileNumber { get; set; }
        public string AddressNotes { get; set; }
    }

    public class UserBookingDTO
    {
        public int BookingId { get; set; }
        public string CustomerName { get; set; }
        public string ServiceName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string PostalCode { get; set; }
        public string Address { get; set; }
        public string WorkDescription { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public int UserId { get; set; }
        public int PaidAmount { get; set; }
        public string ImagePath { get; set; }
    }


    public class BookingDetailsDTO
    {
        public int BookingId { get; set; }
        public string CustomerName { get; set; }
        public string ServiceName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string PostalCode { get; set; }
        public string Address { get; set; }
        public string WorkDescription { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public int UserId { get; set; }
       
    }
}
