using GemCare.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemCare.Data.Repository
{
    public interface IPaymentRepository
    {
        (int status, string message) SaveBookingPaymentInfo(PaymentDTO paymentInfo);
    }
}
