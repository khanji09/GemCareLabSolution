using GemCare.Data.Common;
using GemCare.Data.DTOs;
using GemCare.Data.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemCare.Data.Repository
{
    public class DashboardRepository : BaseRepository, IDashboardRepository
    {
        public DashboardRepository(IConfiguration configuration) : base(configuration)
        {
        }
        public (int status, string message, JobsSummary summary) GetJobsSummary()
        {
            DataTable dt = new();
            JobsSummary _toreturn = new();
            try
            {

                using var dbConnection = new SqlConnection(GetConnectionString());
                dbConnection.Open();
                var sqlCommand = new SqlCommand
                {
                    Connection = dbConnection,
                    CommandText = "sp_Admin_Dashboard_Total_summary",
                    CommandTimeout = DataConstants.CONNECTION_TIMEOUT,
                    CommandType = CommandType.StoredProcedure
                };

                SqlParameter errCodeParam = new("@pErrCode", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                sqlCommand.Parameters.Add(errCodeParam);
                SqlParameter errMessageParam = new("@pErrMessage", SqlDbType.NVarChar, 20)
                {
                    Direction = ParameterDirection.Output
                };
                sqlCommand.Parameters.Add(errMessageParam);

                var sqlAdapter = new SqlDataAdapter(sqlCommand);
                sqlAdapter.Fill(dt);
                errorCode = int.Parse(errCodeParam.Value.ToString());
                errorMessage = errMessageParam.Value.ToString();
                if (errorCode != -1)
                {
                    _toreturn.PendingJobs = int.Parse(dt.Rows[0]["TotalStudents"].ToString());
                    _toreturn.AllocatedJobs = int.Parse(dt.Rows[0]["TotalTutors"].ToString());
                    _toreturn.CompletedJobs = int.Parse(dt.Rows[0]["TotalSubjects"].ToString());
                    _toreturn.CancelledJobs = int.Parse(dt.Rows[0]["TotalSubjects"].ToString());
                }


            }
            catch (Exception)
            {
                throw;
            }

            return (errorCode, errorMessage, _toreturn);
        }
    }
}
