namespace GemCare.API.Contracts.Request
{
    public class UpdateBookingReviewRequest
    {
        public int Id { get; set; }
        public int Reviewpoints { get; set; }
        public string Comments { get; set; }
    }
}
