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
    public class ServiceRepository : BaseRepository, IServiceRepository
    {
        private int _status = -1;
        private string _message = "";
        public ServiceRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public (int status, string message, List<ServiceDTO> services) GetAllServices()
        {
            List<ServiceDTO> toreturn = new List<ServiceDTO>();
            try
            {
                using var dbConnection = new SqlConnection(GetConnectionString());
                dbConnection.Open();
                var sqlCommand = new SqlCommand
                {
                    Connection = dbConnection,
                    CommandText = "spGetAllServices",
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
                //
                SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
                DataTable dt = new DataTable();
                da.Fill(dt);
                //
                _status = int.Parse(errCodeParam.Value.ToString());
                _message = errMessageParam.Value.ToString();

                if (_status > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        toreturn.Add(new ServiceDTO()
                        {
                            Id = int.Parse(row["Id"].ToString()),
                            ImageUrl = row["ImageUrl"].ToString(),
                            IsDeleted = bool.Parse(row["IsDeleted"].ToString()),
                            Description = row["Description"].ToString(),
                            IsActive = bool.Parse(row["IsActive"].ToString()),
                            Name = row["Name"].ToString(),
                            Price = int.Parse(row["Price"].ToString()),
                            ShortDescription = row["ShortDescription"].ToString()
                        });
                    }
                }

            }
            catch { throw; }
            // return data.
            return (_status, _message, toreturn);
        }

        public (int status, string message, ServiceDTO service) GetServiceDetail(int serviceId)
        {
            ServiceDTO toreturn = new ServiceDTO();
            try
            {
                using var dbConnection = new SqlConnection(GetConnectionString());
                dbConnection.Open();
                var sqlCommand = new SqlCommand
                {
                    Connection = dbConnection,
                    CommandText = "spGetServiceById",
                    CommandTimeout = DataConstants.CONNECTION_TIMEOUT,
                    CommandType = CommandType.StoredProcedure
                };

                sqlCommand.Parameters.AddWithValue("@pServiceId", serviceId);

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
                SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
                DataTable dt = new DataTable();
                da.Fill(dt);
                //
                _status = int.Parse(errCodeParam.Value.ToString());
                _message = errMessageParam.Value.ToString();

                if (_status > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        toreturn = new ServiceDTO()
                        {
                            Id = int.Parse(row["Id"].ToString()),
                            ImageUrl = row["ImageUrl"].ToString(),
                            IsDeleted = bool.Parse(row["IsDeleted"].ToString()),
                            Description = row["Description"].ToString(),
                            IsActive = bool.Parse(row["IsActive"].ToString()),
                            Name = row["Name"].ToString(),
                            Price = int.Parse(row["Price"].ToString()),
                            ShortDescription = row["ShortDescription"].ToString()

                        };
                    }
                }
            }
            catch(Exception ae)
            {
                _status = -1;
                _message = ae.Message;
            }
            // return data.
            return (_status, _message, toreturn);
        }
    }
}
