using GemCare.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemCare.Data.Interfaces
{
    public interface IAppointmentRepository
    {
        (int status, string message, List<AppointmentDTO> appointments, int totalPages, int totalRecords) GetAppointments(int type, 
            int pageNumber, int pageSize);
        (int status, string message) AssignJobToTechnician(int bookingId, int technicianId);
        (int status, string message, List<AppointmentDTO> appointments, int totalPages, int totalRecords) GetBookingReviews(int pageNumber, int pageSize);
    }
}
