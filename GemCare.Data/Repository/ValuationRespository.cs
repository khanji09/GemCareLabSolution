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
    public class ValuationRespository : BaseRepository, IValuationRepository
    {
        private int _status;
        private string _message;
        public ValuationRespository(IConfiguration configuration) : base(configuration)
        {
        }

        public (int status, string message, int ValuationId) AddValuation(ValuationDTO model)
        {
            int _ValuationId = 0;
            try
            {
                using var dbConnection = new SqlConnection(GetConnectionString());
                dbConnection.Open();
                var sqlCommand = new SqlCommand
                {
                    Connection = dbConnection,
                    CommandText = "spAddValuation",
                    CommandTimeout = DataConstants.CONNECTION_TIMEOUT,
                    CommandType = CommandType.StoredProcedure
                };

                sqlCommand.Parameters.AddWithValue("@CustomerId", model.CustomerId);
                sqlCommand.Parameters.AddWithValue("@TechnicianId", model.TechnicianId);
                sqlCommand.Parameters.AddWithValue("@ServiceId", model.ServiceId);
                sqlCommand.Parameters.AddWithValue("@ItemDescription", model.ItemDescription);
                sqlCommand.Parameters.AddWithValue("@ImageUrl", model.ImageUrl);
                sqlCommand.Parameters.AddWithValue("@VideoUrl", model.VideoUrl);
                sqlCommand.Parameters.AddWithValue("@Quotation", model.Quotation);


                // out params
                SqlParameter _valid = new("@ValuationID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                sqlCommand.Parameters.Add(_valid);
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
                _status = int.Parse(errCodeParam.Value.ToString());
                _message = errMessageParam.Value.ToString();
                _ValuationId = int.Parse(_valid.Value.ToString());

            }
            catch { throw; }
            // return data.
            return (_status, _message, _ValuationId);
        }

        public (int status, string message, List<AdminValuationDTO>) GetValuationsAdmin()
        {
            List<AdminValuationDTO> toreturn = new List<AdminValuationDTO>();
            try
            {
                using var dbConnection = new SqlConnection(GetConnectionString());
                dbConnection.Open();
                var sqlCommand = new SqlCommand
                {
                    Connection = dbConnection,
                    CommandText = "spGetValuationRequets_Admin",
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
                DateTime _date = DateTime.Now.Date;
                if (_status > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        toreturn.Add(new AdminValuationDTO()
                        {
                            Id = int.Parse(row["Id"].ToString()),                           
                            CustomerId = int.Parse(row["CustomerId"].ToString()),
                            ImageUrl = row["ImageUrl"].ToString(),
                            VideoUrl = row["VideoUrl"].ToString(),
                            ItemDescription = row["ItemDescription"].ToString(),
                            Quotation = int.Parse(row["Quotation"].ToString()),                           
                            TechnicianId = int.Parse(row["TechnicianId"].ToString()),
                            CustomerFirstName = row["CustomerFirstName"].ToString(),
                            CustomerLastName = row["CustomerLastName"].ToString(),
                            ServiceName = row["ServiceName"].ToString(),
                            TechFirstName = row["TechFirstName"].ToString(),
                            TechLastName = row["TechLastName"].ToString()
                        });
                    }
                }
            }
            catch (Exception ae)
            {
                _status = -1;
                _message = ae.Message;
            }
            // return data.
            return (_status, _message, toreturn);
        }
        public (int status, string message, List<AdminValuationDTO>) GetValuationsRequests(int userid,bool isTechnician=false)
        {
            List<AdminValuationDTO> toreturn = new List<AdminValuationDTO>();
            try
            {
                using var dbConnection = new SqlConnection(GetConnectionString());
                dbConnection.Open();
                var sqlCommand = new SqlCommand
                {
                    Connection = dbConnection,
                    CommandText = "spGetValuationRequets",
                    CommandTimeout = DataConstants.CONNECTION_TIMEOUT,
                    CommandType = CommandType.StoredProcedure
                };


                sqlCommand.Parameters.AddWithValue("@pUserId", userid);
                sqlCommand.Parameters.AddWithValue("@pIsTechnician", isTechnician);
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
                DateTime _date = DateTime.Now.Date;
                if (_status > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        toreturn.Add(new AdminValuationDTO()
                        {
                            Id = int.Parse(row["Id"].ToString()),
                            CustomerId = int.Parse(row["CustomerId"].ToString()),
                            ImageUrl = row["ImageUrl"].ToString(),
                            VideoUrl = row["VideoUrl"].ToString(),
                            ItemDescription = row["ItemDescription"].ToString(),
                            Quotation = int.Parse(row["Quotation"].ToString()),
                            TechnicianId = int.Parse(row["TechnicianId"].ToString()),
                            CustomerFirstName = row["CustomerFirstName"].ToString(),
                            CustomerLastName = row["CustomerLastName"].ToString(),
                            ServiceName = row["ServiceName"].ToString(),
                            TechFirstName = row["TechFirstName"].ToString(),
                            TechLastName = row["TechLastName"].ToString()
                        });
                    }
                }
            }
            catch (Exception ae)
            {
                _status = -1;
                _message = ae.Message;
            }
            // return data.
            return (_status, _message, toreturn);
        }
    }
}
