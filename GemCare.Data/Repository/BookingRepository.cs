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
    public class BookingRepository : BaseRepository, IBookingRepository
    {
        private int _status;
        private string _message;
        public BookingRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public (int status, string message, int bookingid) AddBooking(BookingDTO model)
        {
            int _booking_id = 0;
            try
            {
                using var dbConnection = new SqlConnection(GetConnectionString());
                dbConnection.Open();
                var sqlCommand = new SqlCommand
                {
                    Connection = dbConnection,
                    CommandText = "spAddBooking",
                    CommandTimeout = DataConstants.CONNECTION_TIMEOUT,
                    CommandType = CommandType.StoredProcedure
                };

                sqlCommand.Parameters.AddWithValue("@ServiceId", model.ServiceId);
                sqlCommand.Parameters.AddWithValue("@UserId", model.UserId);
                sqlCommand.Parameters.AddWithValue("@Address", model.Address);
                sqlCommand.Parameters.AddWithValue("@MobileNumber", model.MobileNumber);
                sqlCommand.Parameters.AddWithValue("@Name", model.Name);
                sqlCommand.Parameters.AddWithValue("@Email", model.Email);
                sqlCommand.Parameters.AddWithValue("@PostalCode", model.PostalCode);
                sqlCommand.Parameters.AddWithValue("@WorkDescription", model.WorkDescription);
                sqlCommand.Parameters.AddWithValue("@ImagePath", model.ImagePath);
                sqlCommand.Parameters.AddWithValue("@RequiredDate", model.RequiredDate);

                // out params
                SqlParameter _bookingid = new("@BookingID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                sqlCommand.Parameters.Add(_bookingid);
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
                _booking_id = int.Parse(_bookingid.Value.ToString());

            }
            catch { throw; }
            // return data.
            return (_status, _message, _booking_id);
        }

        public (int status, string message, List<UserBookingDTO> bookings) UserCompletedBookings(int userid)
        {
            List<UserBookingDTO> toreturn = new List<UserBookingDTO>();
            try
            {
                using var dbConnection = new SqlConnection(GetConnectionString());
                dbConnection.Open();
                var sqlCommand = new SqlCommand
                {
                    Connection = dbConnection,
                    CommandText = "spUserCompletedBookings",
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
                if (_status > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        toreturn.Add(new UserBookingDTO()
                        {
                            Address = row["Address"].ToString(),
                            BookingId = int.Parse(row["BookingId"].ToString()),
                            CreatedOn = DateTime.Parse(row["CreatedOn"].ToString()),
                            CustomerName = row["CustomerName"].ToString(),
                            Email = row["Email"].ToString(),
                            ImagePath = row["ImagePath"].ToString(),
                            MobileNumber = row["MobileNumber"].ToString(),
                            PaidAmount = int.Parse(row["PaidAmount"].ToString()),
                            ExpectedDate = DateTime.TryParse(row["ExpectedDate"].ToString(), out _date) ? _date : DateTime.Today.AddDays(7),
                            RequiredDate = DateTime.TryParse(row["RequiredDate"].ToString(), out _date) ? _date : DateTime.Today.AddDays(7),
                            PostalCode = row["PostalCode"].ToString(),
                            ServiceName = row["ServiceName"].ToString(),
                            UserId = int.Parse(row["UserId"].ToString()),
                            WorkDescription = row["WorkDescription"].ToString()
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

        public (int status, string message, List<UserBookingDTO> bookings) UserUpComingBookings(int userid)
        {
            List<UserBookingDTO> toreturn = new List<UserBookingDTO>();
            try
            {
                using var dbConnection = new SqlConnection(GetConnectionString());
                dbConnection.Open();
                var sqlCommand = new SqlCommand
                {
                    Connection = dbConnection,
                    CommandText = "spUserUpComingBookings",
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
                if (_status > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        toreturn.Add(new UserBookingDTO()
                        {
                            Address = row["Address"].ToString(),
                            BookingId = int.Parse(row["BookingId"].ToString()),
                            CreatedOn = DateTime.Parse(row["CreatedOn"].ToString()),
                            CustomerName = row["CustomerName"].ToString(),
                            Email = row["Email"].ToString(),
                            ImagePath = row["ImagePath"].ToString(),
                            MobileNumber = row["MobileNumber"].ToString(),
                            PaidAmount = int.Parse(row["PaidAmount"].ToString()),
                            PostalCode = row["PostalCode"].ToString(),
                            ExpectedDate = DateTime.TryParse(row["ExpectedDate"].ToString(), out _date) ? _date : DateTime.Today.AddDays(7),
                            RequiredDate = DateTime.TryParse(row["RequiredDate"].ToString(), out _date) ? _date : DateTime.Today.AddDays(7),
                            ServiceName = row["ServiceName"].ToString(),
                            UserId = int.Parse(row["UserId"].ToString()),
                            WorkDescription = row["WorkDescription"].ToString()
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


        public (int status, string message, BookingDetailsDTO booking) BookingDetails(int BookingId)
        {
            BookingDetailsDTO toreturn = new BookingDetailsDTO();
            try
            {
                using var dbConnection = new SqlConnection(GetConnectionString());
                dbConnection.Open();
                var sqlCommand = new SqlCommand
                {
                    Connection = dbConnection,
                    CommandText = "spBookingDetails",
                    CommandTimeout = DataConstants.CONNECTION_TIMEOUT,
                    CommandType = CommandType.StoredProcedure
                };

                sqlCommand.Parameters.AddWithValue("@BookingId", BookingId);

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
                if (_status > 0 && dt.Rows.Count>0)
                {
                    DataRow row = dt.Rows[0];
                    toreturn = new BookingDetailsDTO()
                    {
                        Address = row["Address"].ToString(),
                        BookingId = int.Parse(row["BookingId"].ToString()),
                        CreatedOn = DateTime.Parse(row["CreatedOn"].ToString()),
                        CustomerName = row["CustomerName"].ToString(),
                        Email = row["Email"].ToString(),
                        MobileNumber = row["MobileNumber"].ToString(),
                        PostalCode = row["PostalCode"].ToString(),
                        ExpectedDate = DateTime.TryParse(row["ExpectedDate"].ToString(), out _date) ? _date : DateTime.Today.AddDays(7),
                        RequiredDate = DateTime.TryParse(row["RequiredDate"].ToString(), out _date) ? _date : DateTime.Today.AddDays(7),
                        ServiceName = row["ServiceName"].ToString(),
                        UserId = int.Parse(row["UserId"].ToString()),
                        WorkDescription = row["WorkDescription"].ToString()
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

        public (int status, string message, List<UserBookingDTO> bookings) GetTechnicianUpcomingBookings(int technicianId)
        {
            List<UserBookingDTO> toreturn = new();
            try
            {
                using var dbConnection = new SqlConnection(GetConnectionString());
                dbConnection.Open();
                var sqlCommand = new SqlCommand
                {
                    Connection = dbConnection,
                    CommandText = "spTechnicianUpComingBookings",
                    CommandTimeout = DataConstants.CONNECTION_TIMEOUT,
                    CommandType = CommandType.StoredProcedure
                };

                sqlCommand.Parameters.AddWithValue("@pTechnicianId", technicianId);

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
                SqlDataAdapter da = new(sqlCommand);
                DataTable dt = new();
                da.Fill(dt);
                //
                errorCode = int.Parse(errCodeParam.Value.ToString());
                errorMessage = errMessageParam.Value.ToString();
                DateTime _date = DateTime.Now.Date;
                if (errorCode > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        toreturn.Add(new UserBookingDTO()
                        {
                            Address = row["Address"].ToString(),
                            BookingId = int.Parse(row["BookingId"].ToString()),
                            CreatedOn = DateTime.Parse(row["CreatedOn"].ToString()),
                            CustomerName = row["CustomerName"].ToString(),
                            Email = row["Email"].ToString(),
                            ImagePath = row["ImagePath"].ToString(),
                            MobileNumber = row["MobileNumber"].ToString(),
                            PaidAmount = int.Parse(row["PaidAmount"].ToString()),
                            PostalCode = row["PostalCode"].ToString(),
                            ExpectedDate = DateTime.TryParse(row["ExpectedDate"].ToString(), out _date) ? _date : DateTime.Today.AddDays(7),
                            RequiredDate = DateTime.TryParse(row["RequiredDate"].ToString(), out _date) ? _date : DateTime.Today.AddDays(7),
                            ServiceName = row["ServiceName"].ToString(),
                            UserId = int.Parse(row["UserId"].ToString()),
                            WorkDescription = row["WorkDescription"].ToString()
                        });
                    }
                }
            }
            catch
            {
                throw;
            }
            // return data.
            return (errorCode, errorMessage, toreturn);
        }

        public (int status, string message, List<UserBookingDTO> bookings) GetTechnicianCompletedBookings(int technicianId)
        {
            List<UserBookingDTO> toreturn = new();
            try
            {
                using var dbConnection = new SqlConnection(GetConnectionString());
                dbConnection.Open();
                var sqlCommand = new SqlCommand
                {
                    Connection = dbConnection,
                    CommandText = "spTechnicianUpComingBookings",
                    CommandTimeout = DataConstants.CONNECTION_TIMEOUT,
                    CommandType = CommandType.StoredProcedure
                };

                sqlCommand.Parameters.AddWithValue("@pTechnicianId", technicianId);

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
                SqlDataAdapter da = new(sqlCommand);
                DataTable dt = new();
                da.Fill(dt);
                //
                errorCode = int.Parse(errCodeParam.Value.ToString());
                errorMessage = errMessageParam.Value.ToString();
                DateTime _date = DateTime.Now.Date;
                if (errorCode > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        toreturn.Add(new UserBookingDTO()
                        {
                            Address = row["Address"].ToString(),
                            BookingId = int.Parse(row["BookingId"].ToString()),
                            CreatedOn = DateTime.Parse(row["CreatedOn"].ToString()),
                            CustomerName = row["CustomerName"].ToString(),
                            Email = row["Email"].ToString(),
                            ImagePath = row["ImagePath"].ToString(),
                            MobileNumber = row["MobileNumber"].ToString(),
                            PaidAmount = int.Parse(row["PaidAmount"].ToString()),
                            PostalCode = row["PostalCode"].ToString(),
                            ExpectedDate = DateTime.TryParse(row["ExpectedDate"].ToString(), out _date) ? _date : DateTime.Today.AddDays(7),
                            RequiredDate = DateTime.TryParse(row["RequiredDate"].ToString(), out _date) ? _date : DateTime.Today.AddDays(7),
                            ServiceName = row["ServiceName"].ToString(),
                            UserId = int.Parse(row["UserId"].ToString()),
                            WorkDescription = row["WorkDescription"].ToString()
                        });
                    }
                }
            }
            catch
            {
                throw;
            }
            // return data.
            return (errorCode, errorMessage, toreturn);
        }
    }
}
