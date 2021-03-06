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

        public (int status, string message) InsertUpdatePayPalPaymentInfo(InsertUpdatePayPalInfoDTO paymentDTO)
        {
            try
            {
                using var dbConnection = new SqlConnection(GetConnectionString());
                dbConnection.Open();
                var sqlCommand = new SqlCommand
                {
                    Connection = dbConnection,
                    CommandText = "spInsertUpdatePaypalPaymentInfo",
                    CommandTimeout = DataConstants.CONNECTION_TIMEOUT,
                    CommandType = CommandType.StoredProcedure
                };

                sqlCommand.Parameters.AddWithValue("@BookingId", paymentDTO.BookingId);
                sqlCommand.Parameters.AddWithValue("@PaidAmount", paymentDTO.PaidAmount);
                sqlCommand.Parameters.AddWithValue("@OrderId", paymentDTO.OrderId);
                sqlCommand.Parameters.AddWithValue("@PaypalRequestId", paymentDTO.PaypalRequestId);
                sqlCommand.Parameters.AddWithValue("@PayerId", paymentDTO.PayerId);
                sqlCommand.Parameters.AddWithValue("@Token", paymentDTO.Token);
                sqlCommand.Parameters.AddWithValue("@fee", paymentDTO.Fee);

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

        public (int status, string message) SavePayPalPaymentInfo(PayPalPaymentDTO  payPalPaymentDTO)
        {
            try
            {
                using var dbConnection = new SqlConnection(GetConnectionString());
                dbConnection.Open();
                var sqlCommand = new SqlCommand
                {
                    Connection = dbConnection,
                    CommandText = "spSavePayPalPayment",
                    CommandTimeout = DataConstants.CONNECTION_TIMEOUT,
                    CommandType = CommandType.StoredProcedure
                };

                sqlCommand.Parameters.AddWithValue("@BookingId",  payPalPaymentDTO.BookingId);
                sqlCommand.Parameters.AddWithValue("@PaidAmount",  payPalPaymentDTO.PaidAmount);
                sqlCommand.Parameters.AddWithValue("@OrderId", payPalPaymentDTO.OrderId);
                sqlCommand.Parameters.AddWithValue("@PaypalRequestId", payPalPaymentDTO.PaypalRequestId);
                
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

        public (int status, string message) UpdatePayPalPaymentInfo(UpdatePayPalInfoDTO updatePayPalPaymentDTO)
        {
            try
            {
                using var dbConnection = new SqlConnection(GetConnectionString());
                dbConnection.Open();
                var sqlCommand = new SqlCommand
                {
                    Connection = dbConnection,
                    CommandText = "spUpdatePayPalPaymentInfo",
                    CommandTimeout = DataConstants.CONNECTION_TIMEOUT,
                    CommandType = CommandType.StoredProcedure
                };

                sqlCommand.Parameters.AddWithValue("@PayerId", updatePayPalPaymentDTO.PayerId);
                sqlCommand.Parameters.AddWithValue("@Token", updatePayPalPaymentDTO.Token);
                sqlCommand.Parameters.AddWithValue("@OrderId", updatePayPalPaymentDTO.OrderId);
                sqlCommand.Parameters.AddWithValue("@PaypalRequestId", updatePayPalPaymentDTO.PaypalRequestId);
                sqlCommand.Parameters.AddWithValue("@fee", updatePayPalPaymentDTO.fee);

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
