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
                    CommandText = "spAdmin_JobsSummary",
                    CommandTimeout = DataConstants.CONNECTION_TIMEOUT,
                    CommandType = CommandType.StoredProcedure
                };

                SqlParameter errCodeParam = new("@pErrCode", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                sqlCommand.Parameters.Add(errCodeParam);
                SqlParameter errMessageParam = new("@pErrMessage", SqlDbType.NVarChar, 200)
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
                    _toreturn.UnallocatedJobs = int.Parse(dt.Rows[0]["UnallocatedJobs"].ToString());
                    _toreturn.AllocatedJobs = int.Parse(dt.Rows[0]["AllocatedJobs"].ToString());
                    _toreturn.CompletedJobs = int.Parse(dt.Rows[0]["CompletedJobs"].ToString());
                    _toreturn.CancelledJobs = int.Parse(dt.Rows[0]["CancelledJobs"].ToString());
                }


            }
            catch (Exception)
            {
                throw;
            }

            return (errorCode, errorMessage, _toreturn);
        }

        public (int status, string message, List<AppUser> technicians) GetTechnicians()
        {
            List<AppUser> technicianList = null;
            DataTable dt = new();
            try
            {
                {
                    using var dbConnection = new SqlConnection(GetConnectionString());
                    dbConnection.Open();
                    var sqlCommand = new SqlCommand
                    {
                        Connection = dbConnection,
                        CommandText = "spGetTechnicians",
                        CommandTimeout = DataConstants.CONNECTION_TIMEOUT,
                        CommandType = CommandType.StoredProcedure
                    };

                    SqlParameter errCodeParam = new("@pErrCode", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    sqlCommand.Parameters.Add(errCodeParam);
                    SqlParameter errMessageParam = new("@pErrMessage", SqlDbType.NVarChar, DataConstants.ERRMESSAGE_LENGTH)
                    {
                        Direction = ParameterDirection.Output
                    };
                    sqlCommand.Parameters.Add(errMessageParam);

                    var sqlAdapter = new SqlDataAdapter(sqlCommand);
                    sqlAdapter.Fill(dt);
                    errorCode = int.Parse(errCodeParam.Value.ToString());
                    errorMessage = errMessageParam.Value.ToString();
                }
                if (dt?.Rows.Count > 0)
                {
                    technicianList = new List<AppUser>();
                    foreach (DataRow row in dt.Rows)
                    {
                        AppUser tempObj = new()
                        {
                            Id = int.Parse(row["Id"].ToString()),
                            FirstName = row["FirstName"].ToString(),
                            LastName = row["LastName"].ToString(),
                            Email = row["Email"].ToString(),
                            Mobile = row["Mobile"].ToString()
                        };
                        technicianList.Add(tempObj);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return (errorCode, errorMessage, technicianList);
        }
    }
}
