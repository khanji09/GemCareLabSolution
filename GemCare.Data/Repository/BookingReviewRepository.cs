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
    public class BookingReviewRepository : BaseRepository, IBookingReviewRepository
    {
        private int _status;
        private string _message;
        public BookingReviewRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public (int status, string message, List<BookingReviewDTO>) GetBookingReview(int bookingid)
        {
            List<BookingReviewDTO> toreturn = new List<BookingReviewDTO>();
            try
            {
                using var dbConnection = new SqlConnection(GetConnectionString());
                dbConnection.Open();
                var sqlCommand = new SqlCommand
                {
                    Connection = dbConnection,
                    CommandText = "spGetBookingReview",
                    CommandTimeout = DataConstants.CONNECTION_TIMEOUT,
                    CommandType = CommandType.StoredProcedure
                };

                sqlCommand.Parameters.AddWithValue("@BookingId", bookingid);

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
                        toreturn.Add(new BookingReviewDTO()
                        {
                            Id = int.Parse(row["Id"].ToString()),
                            BookingId = int.Parse(row["BookingId"].ToString()),
                            Comments = row["Comments"].ToString(),
                            CreatedOn = DateTime.Parse(row["CreatedOn"].ToString()),
                            IsRead = bool.Parse(row["IsRead"].ToString()),
                            ReviewPoints = int.Parse(row["ReviewPoints"].ToString()),
                            UpdatedOn = DateTime.Parse(row["UpdatedOn"].ToString())
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

        public (int status, string message) UpdateBookingReview(BookingReviewDTO model)
        {
            try
            {
                using var dbConnection = new SqlConnection(GetConnectionString());
                dbConnection.Open();
                var sqlCommand = new SqlCommand
                {
                    Connection = dbConnection,
                    CommandText = "spUpdateBookingReview",
                    CommandTimeout = DataConstants.CONNECTION_TIMEOUT,
                    CommandType = CommandType.StoredProcedure
                };

                sqlCommand.Parameters.AddWithValue("@Id", model.Id);
                sqlCommand.Parameters.AddWithValue("@ReviewPoints", model.ReviewPoints);
                sqlCommand.Parameters.AddWithValue("@Comments", model.Comments);
                sqlCommand.Parameters.AddWithValue("@IsRead", model.IsRead);
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

        public (int status, string message, UnReadBookingReviewDTO review) GetUnReadBookingReviewByCustomer(int userid)
        {
            UnReadBookingReviewDTO toreturn = new UnReadBookingReviewDTO();
            try
            {
                using var dbConnection = new SqlConnection(GetConnectionString());
                dbConnection.Open();
                var sqlCommand = new SqlCommand
                {
                    Connection = dbConnection,
                    CommandText = "spGetUnreadBookingReviewByCustomer",
                    CommandTimeout = DataConstants.CONNECTION_TIMEOUT,
                    CommandType = CommandType.StoredProcedure
                };

                sqlCommand.Parameters.AddWithValue("@UserId", userid);

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
                if (_status > 0 && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    toreturn = new UnReadBookingReviewDTO()
                    {
                        Id = int.Parse(row["Id"].ToString()),
                        BookingId = int.Parse(row["BookingId"].ToString()),
                        Comments = row["Comments"].ToString(),
                        CreatedOn = DateTime.Parse(row["CreatedOn"].ToString()),
                        IsRead = bool.Parse(row["IsRead"].ToString()),
                        ReviewPoints = int.Parse(row["ReviewPoints"].ToString()),
                        UpdatedOn = DateTime.Parse(row["UpdatedOn"].ToString()),
                        ServiceName = row["ServiceName"].ToString(),
                        ShortDescription = row["ShortDescription"].ToString()
                    };

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
