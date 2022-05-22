using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemCare.Data.DTOs
{
    public class PaymentDTO:BaseEntity
    {
        public int BookingId { get; set; }
        public string CustomerId { get; set; }
        public string PaymentMethodId { get; set; }
        public string TransactionId { get; set; }
        public string ChargeId { get; set; }
        public decimal Amount { get; set; }       
        public bool IsRefund { get; set; }
        public DateTime? RefundDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn{get;set;}
    }
}
