namespace GemCare.API.Contracts.Response
{
    public class UnReadBookingReviewResponse
    {
        public int Id { get; set; }
        public int Bookingid { get; set; }
        public int ReviewPoints { get; set; }
        public string Comments { get; set; }
        public bool IsRead { get; set; }
        public string Servicename { get; set; }
        public string Shortdescription { get; set; }
    }
}
