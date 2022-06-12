using System.Collections.Generic;

namespace GemCare.API.Contracts.Response
{
    public class CustomerBookingReviewResponse
    {
        public int Id { get; set; }
        public int Bookingid { get; set; }
        public int Reviewpoints { get; set; }
        public string Comments { get; set; }
        public bool Isread { get; set; }
    }

    public class CustomerBookingReviewResponseRoot
    {
        public List<CustomerBookingReviewResponse> BookingReviews { get; set; }
        
    }
}
