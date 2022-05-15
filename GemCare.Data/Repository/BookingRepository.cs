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
    public class BookingRepository: BaseRepository,IBookingRepository
    {
        private int _status;
        private string _message;
        public BookingRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public (int status, string message,int bookingid) AddBooking(BookingDTO model)
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
                sqlCommand.Parameters.AddWithValue("@AddresId", model.AddressId);
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
    }
}
