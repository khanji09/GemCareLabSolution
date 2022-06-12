using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemCare.Data.DTOs
{
    public class BookingReviewDTO
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public int ReviewPoints { get; set; }
        public string Comments { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
