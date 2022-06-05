using GemCare.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemCare.Data.Interfaces
{
    public interface IServiceRepository
    {
        (int status, string message,List<ServiceDTO> services) GetAllServices();
        (int status, string message,ServiceDTO service) GetServiceDetail(int serviceId);
        (int status, string message, List<ServiceDTO> services) GetSubServices(int serviceId);
    }
}
