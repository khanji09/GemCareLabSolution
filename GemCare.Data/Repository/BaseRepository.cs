using GemCare.Data.Common;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemCare.Data.Repository
{
    public abstract class BaseRepository
    {
        private readonly IConfiguration _configuration;
        protected int errorCode;
        protected string errorMessage;
        public BaseRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            InitConfiguration();
        }

        protected static string GetConnectionString()
        {
            return AppSettings.Instance.GetConnection("DBConnection_Live");
        }

        private void InitConfiguration()
        {
            AppSettings.Instance.SetConfiguration(_configuration);
        }
    }
}
