using System;
using System.Collections.Generic;

namespace GemCare.API.Contracts.Response
{
    public class BookingResponse
    {
        public int Bookingid { get; set; }

    }
    public class AddBookingResponse:BookingResponse
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public int Userid { get; set; }
        public int Serviceid { get; set; }
        public string Address { get; set; }
        public string Workdescription { get; set; }
        public DateTime Requireddate { get; set; }
    }
    public class UserBookingResponse:BookingResponse
    {        
        public string Customername { get; set; }
        public string Servicename { get; set; }
        public string Email { get; set; }
        public string Mobilenumber { get; set; }
        public string Postalcode { get; set; }
        public string Address { get; set; }
        public string Workdescription { get; set; }
        public DateTime Createdon { get; set; }
        public DateTime? Requireddate { get; set; }
        public DateTime? Expecteddate { get; set; }
        public int Userid { get; set; }
        public int Paidamount { get; set; }
        public List<string> Imagespath { get; set; }
    }
    public class UserCompleteBookingsResponse
    {
        public List<UserBookingResponse>   CompletedBookings { get; set; }
    }
    public class UserUpCommingBookingsResponse 
    {
        public List<UserBookingResponse> UpComingBookings { get; set; }
    }
}
