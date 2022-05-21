using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GemCare.API.Contracts.Response
{
    public class AppointmentResponse
    {
        public int Id { get; set; }
        public string Customername { get; set; }
        public string Servicename { get; set; }
        public string Technicianname { get; set; }
        public bool Iscompleted { get; set; }
        public bool Iscancelled { get; set; }
    }

    public class BookingReviewResponse
    {
        public int Id { get; set; }
        public string Customername { get; set; }
        public string Servicename { get; set; }
        public string Technicianname { get; set; }
        public int Points { get; set; }
        public string Comments { get; set; }
    }
}
