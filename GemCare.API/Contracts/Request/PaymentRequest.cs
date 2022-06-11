using GemCare.API.Common;
using System.ComponentModel.DataAnnotations;

namespace GemCare.API.Contracts.Request
{
    public class PaymentMethodCreateRequest
    {
        [Required]
        public string Stripetoken { get; set; }
    }
        public abstract class PaymentRequest 
        {
            [Required]
            [GreaterThanZero]
            public int Bookingid { get; set; }

            [Required]
            public string Paymentmethodid { get; set; }

            [Required]
            [GreaterThanZero]
            public decimal Amount { get; set; }
        }

        public class CapturePaymentRequest : PaymentRequest
        {
            [Required]
            [StringLength(3, MinimumLength = 3)]
            public string Currency { get; set; }

            [Required]
            [EmailAddress]
            public string Useremail { get; set; }
        }

        public class SavePaymentRequest : PaymentRequest
        {
            public string Customerid { get; set; }

            [Required]
            public string Transactionid { get; set; }

            [Required]
            public string Chargeid { get; set; }
           
        }

    }

