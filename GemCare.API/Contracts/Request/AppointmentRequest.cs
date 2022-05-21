using GemCare.API.Common;

namespace GemCare.API.Contracts.Request
{
    public class AssignJobRequest
    {
        [GreaterThanZero(ErrorMessage = "Bookingid must be greater than 0")]
        public int Bookingid { get; set; }

        [GreaterThanZero(ErrorMessage = "Technicianid must be greater than 0")]
        public int Technicianid { get; set; }
    }
}
