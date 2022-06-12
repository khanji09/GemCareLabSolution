using GemCare.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemCare.Data.Interfaces
{
     public interface IBookingReviewRepository
    {
        (int status, string message,List<BookingReviewDTO>) GetBookingReview(int bookingid);
        (int status, string message) UpdateBookingReview(BookingReviewDTO model);
        (int status, string message, UnReadBookingReviewDTO review) GetUnReadBookingReviewByCustomer(int userid);

    }
}
