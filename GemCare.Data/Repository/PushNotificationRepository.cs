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
    public class PushNotificationRepository : BaseRepository, IPushNotificationRepository
    {
        public PushNotificationRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public (int status, string message) SavePushToken(int userId, string pushToken, string deviceId, string devicePlatform)
        {
            try
            {
                using var dbConnection = new SqlConnection(GetConnectionString());
                dbConnection.Open();
                var sqlCommand = new SqlCommand
                {
                    Connection = dbConnection,
                    CommandText = "spSaveUserPushToken",
                    CommandTimeout = 240,
                    CommandType = CommandType.StoredProcedure
                };

                sqlCommand.Parameters.AddWithValue("@pUserId", userId);
                sqlCommand.Parameters.AddWithValue("@pDeviceId", deviceId);
                sqlCommand.Parameters.AddWithValue("@pPushToken", pushToken);
                sqlCommand.Parameters.AddWithValue("@pDevicePlatform", devicePlatform);

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
            catch
            {
                throw;
            }

            return (errorCode, errorMessage);
        }

        public (int status, string message, PushDTO deviceInfo) GetAdminDeviceInfoForBookingNotification(int bookingid)
        {
            PushDTO pushData = null;
            try
            {
                using var dbConnection = new SqlConnection(GetConnectionString());
                dbConnection.Open();
                var sqlCommand = new SqlCommand
                {
                    Connection = dbConnection,
                    CommandText = "spGetAdminDeviceInfoForBookingNotification",
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
                SqlDataAdapter da = new(sqlCommand);
                DataTable dt = new();
                da.Fill(dt);
                //
                errorCode = int.Parse(errCodeParam.Value.ToString());
                errorMessage = errMessageParam.Value.ToString();
                //
                if (errorCode > 0)
                {
                    pushData = new();
                    foreach (DataRow row in dt.Rows)
                    {
                        pushData.PushToken = row["PushToken"].ToString();
                        pushData.PushTitle = row["PushTitle"].ToString();
                        pushData.PushBody = row["PushBody"].ToString();
                        pushData.DevicePlatform = row["DevicePlatform"].ToString();
                    }
                }
            }
            catch
            {
                throw;
            }
            // return data.
            return (errorCode, errorMessage, pushData);
        }

        public (int status, string message) UpdateBookingNotificationStatus()
        {
            try
            {
                using var dbConnection = new SqlConnection(GetConnectionString());
                dbConnection.Open();
                var sqlCommand = new SqlCommand
                {
                    Connection = dbConnection,
                    CommandText = "spUpdateBookingNotificationStatus",
                    CommandTimeout = 240,
                    CommandType = CommandType.StoredProcedure
                };
                //
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
            catch
            {
                throw;
            }

            return (errorCode, errorMessage);
        }
    }
}
