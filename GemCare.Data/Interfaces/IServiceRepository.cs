using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemCare.Data.Interfaces
{
    public interface IServiceRepository
    {
        (int status, string message) GetAllServices();
        (int status, string message) GetServiceDetail(int serviceId);
    }
}
