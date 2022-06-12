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
        (int status, string message) SavePayPalPaymentInfo(PayPalPaymentDTO payPalPaymentDTO);
        (int status, string message) UpdatePayPalPaymentInfo(UpdatePayPalInfoDTO updatePayPalPaymentDTO);
        (int status, string message) InsertUpdatePayPalPaymentInfo(InsertUpdatePayPalInfoDTO paymentDTO);
    }
}
