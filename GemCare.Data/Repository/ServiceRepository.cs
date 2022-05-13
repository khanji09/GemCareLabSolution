using GemCare.Data.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemCare.Data.Repository
{
    public class ServiceRepository : BaseRepository, IServiceRepository
    {
        public ServiceRepository(IConfiguration configuration) : base(configuration)
        {
        }
        public (int status, string message) GetAllServices()
        {
            throw new NotImplementedException();
        }

        public (int status, string message) GetServiceDetail(int serviceId)
        {
            throw new NotImplementedException();
        }
    }
}
