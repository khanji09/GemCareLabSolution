using GemCare.Data.Common;
using GemCare.Data.DTOs;
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
    public class PaymentRepository : BaseRepository, IPaymentRepository
    {
        //private int errorCode;
        //private string errorMessage;
        public PaymentRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public (int status, string message) SaveBookingPaymentInfo(PaymentDTO paymentInfo)
        {
            try
            {
                using var dbConnection = new SqlConnection(GetConnectionString());
                dbConnection.Open();
                var sqlCommand = new SqlCommand
                {
                    Connection = dbConnection,
                    CommandText = "spInsertUpdateBookingPayment",
                    CommandTimeout = DataConstants.CONNECTION_TIMEOUT,
                    CommandType = CommandType.StoredProcedure
                };

                sqlCommand.Parameters.AddWithValue("@pBookingId", paymentInfo.BookingId);
                sqlCommand.Parameters.AddWithValue("@pPGCustomerId", paymentInfo.CustomerId);
                sqlCommand.Parameters.AddWithValue("@pPaymentMethodId", paymentInfo.PaymentMethodId);
                sqlCommand.Parameters.AddWithValue("@pTransactionId", paymentInfo.TransactionId);
                sqlCommand.Parameters.AddWithValue("@pChargeId", paymentInfo.ChargeId);
                sqlCommand.Parameters.AddWithValue("@pAmount", paymentInfo.Amount);       
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
                //
                sqlCommand.ExecuteNonQuery();
                //
                errorCode = int.Parse(errCodeParam.Value.ToString());
                errorMessage = errMessageParam.Value.ToString();
            }
            catch
            {
                throw;
            }
            //
            return (errorCode, errorMessage);
        }

    }
}
