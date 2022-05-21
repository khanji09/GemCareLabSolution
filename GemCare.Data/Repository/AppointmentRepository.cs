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
    public class AppointmentRepository : BaseRepository, IAppointmentRepository
    {
        public AppointmentRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public (int status, string message) AssignJobToTechnician(int bookingId, int technicianId)
        {
            try
            {
                using var dbConnection = new SqlConnection(GetConnectionString());
                dbConnection.Open();
                var sqlCommand = new SqlCommand
                {
                    Connection = dbConnection,
                    CommandText = "spAssignJob",
                    CommandTimeout = DataConstants.CONNECTION_TIMEOUT,
                    CommandType = CommandType.StoredProcedure
                };

                sqlCommand.Parameters.AddWithValue("@pBookingId", bookingId);
                sqlCommand.Parameters.AddWithValue("@pTechnicianId", technicianId);
                // out params
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
                //
                sqlCommand.ExecuteNonQuery();
                //
                errorCode = int.Parse(errCodeParam.Value.ToString());
                errorMessage = errMessageParam.Value.ToString();

            }
            catch { throw; }
            // return data.
            return (errorCode, errorMessage);
        }

        public (int status, string message, List<AppointmentDTO> appointments, int totalPages, int totalRecords) GetAppointments(int type, int pageNumber, int pageSize)
        {
            List<AppointmentDTO> appointmentList = null;
            DataTable dt = new();
            int totalPages = 0, totalRecords=0;
            try
            {
                {
                    using var dbConnection = new SqlConnection(GetConnectionString());
                    dbConnection.Open();
                    var sqlCommand = new SqlCommand
                    {
                        Connection = dbConnection,
                        CommandText = "spAdmin_GetBookings",
                        CommandTimeout = DataConstants.CONNECTION_TIMEOUT,
                        CommandType = CommandType.StoredProcedure
                    };
                    sqlCommand.Parameters.AddWithValue("@pType", type);
                    sqlCommand.Parameters.AddWithValue("@pPageNumber", pageNumber);
                    sqlCommand.Parameters.AddWithValue("@pPageSize", pageSize);
                    SqlParameter totalCount = new("@pTotalCount", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    sqlCommand.Parameters.Add(totalCount);

                    var sqlAdapter = new SqlDataAdapter(sqlCommand);
                    sqlAdapter.Fill(dt);
                    //errorCode = int.Parse(errCodeParam.Value.ToString());
                    //errorMessage = errMessageParam.Value.ToString();
                    totalRecords = int.Parse(totalCount.Value.ToString());
                    if (pageSize <= 0)
                        pageSize = 1;
                    totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
                }
                if (dt?.Rows.Count > 0)
                {
                    errorCode = 1;
                    errorMessage = "Success";
                    appointmentList = new List<AppointmentDTO>(pageSize);
                    foreach (DataRow row in dt.Rows)
                    {
                        AppointmentDTO tempObj = new()
                        {
                            BookingId = int.Parse(row["Id"].ToString()),
                            CustomerName = row["CustomerName"].ToString(),
                            ServiceName = row["ServiceName"].ToString(),
                            TechnicianName = row["TechnicianName"].ToString(),
                            IsCancelled = bool.Parse(row["IsCancelled"].ToString()),
                            IsCompleted = bool.Parse(row["IsCompleted"].ToString())
                        };
                        appointmentList.Add(tempObj);
                    }
                }
                else
                {
                    errorCode = -1;
                    errorMessage = "No data found";
                }
            }
            catch (Exception)
            {
                throw;
            }

            return (errorCode, errorMessage, appointmentList, totalPages, totalRecords);
        }

        public (int status, string message, List<AppointmentDTO> appointments, int totalPages, int totalRecords) GetBookingReviews(int pageNumber, int pageSize)
        {
            List<AppointmentDTO> appointmentList = null;
            DataTable dt = new();
            int totalPages = 0, totalRecords = 0;
            try
            {
                {
                    using var dbConnection = new SqlConnection(GetConnectionString());
                    dbConnection.Open();
                    var sqlCommand = new SqlCommand
                    {
                        Connection = dbConnection,
                        CommandText = "spGetBookingReviews",
                        CommandTimeout = DataConstants.CONNECTION_TIMEOUT,
                        CommandType = CommandType.StoredProcedure
                    };
                    sqlCommand.Parameters.AddWithValue("@pPageNumber", pageNumber);
                    sqlCommand.Parameters.AddWithValue("@pPageSize", pageSize);
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
                    DataRow row1 = dt.Rows[0];
                    totalRecords = int.Parse(row1["TotalRecords"].ToString());
                    if (pageSize <= 0)
                        pageSize = 1;
                    totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
                    appointmentList = new List<AppointmentDTO>(pageSize);
                    foreach (DataRow row in dt.Rows)
                    {
                        AppointmentDTO tempObj = new()
                        {
                            BookingId = int.Parse(row["Id"].ToString()),
                            CustomerName = row["CustomerName"].ToString(),
                            ServiceName = row["ServiceName"].ToString(),
                            TechnicianName = row["TechnicianName"].ToString(),
                            ReviewPoints = int.Parse(row["ReviewPoints"].ToString()),
                            Comments = row["Comments"].ToString()
                        };
                        appointmentList.Add(tempObj);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return (errorCode, errorMessage, appointmentList, totalPages, totalRecords);
        }
    }
}
