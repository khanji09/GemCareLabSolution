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
    public class UserRepository : BaseRepository, IUserRepository
    {
        private int _status;
        private string _message;
        public UserRepository(IConfiguration configuration) : base(configuration)
        {
        }
        public (int status, string message, UserProfileDTO profile) GetUserPRofile(int id)
        {
            UserProfileDTO toreturn = new UserProfileDTO();
            try
            {
                using var dbConnection = new SqlConnection(GetConnectionString());
                dbConnection.Open();
                var sqlCommand = new SqlCommand
                {
                    Connection = dbConnection,
                    CommandText = "spGetUserProfile",
                    CommandTimeout = DataConstants.CONNECTION_TIMEOUT,
                    CommandType = CommandType.StoredProcedure
                };

                sqlCommand.Parameters.AddWithValue("@UserId", id);

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

                if (_status > 0 && dt.Rows.Count>0)
                {
                    DataRow row = dt.Rows[0];
                    toreturn = new UserProfileDTO()
                    {
                        FullName = row["FullName"].ToString(),
                        FirstName = row["FirstName"].ToString(),
                        DOB = DateTime.Parse(row["DOB"].ToString()),
                        Email = row["Email"].ToString(),
                        Gender = row["Gender"].ToString(),
                        Id = int.Parse(row["Id"].ToString()),
                        ImagePath = row["ImagePath"].ToString(),
                        IsAccountConfirmed = (bool)row["IsAccountConfirmed"],
                        IsActive = (bool)row["IsActive"],
                        IsDeleted = (bool)row["IsDeleted"],
                        LastName = row["LastName"].ToString(),
                        Mobile = row["Mobile"].ToString(),
                        UserTypeId = int.Parse(row["UserTypeId"].ToString())
                    };
                }

            }
            catch { throw; }
            // return data.
            return (_status, _message, toreturn);
        }

        public (int status, string message) UpdateProfileImage(string ImageUrl, int Userid)
        {
            try
            {
                using var dbConnection = new SqlConnection(GetConnectionString());
                dbConnection.Open();
                var sqlCommand = new SqlCommand
                {
                    Connection = dbConnection,
                    CommandText = "spUpdateProfileImage",
                    CommandTimeout = DataConstants.CONNECTION_TIMEOUT,
                    CommandType = CommandType.StoredProcedure
                };

                sqlCommand.Parameters.AddWithValue("@UserId", Userid);
                sqlCommand.Parameters.AddWithValue("@ImagePath", ImageUrl);
               

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
                int effectedrows = sqlCommand.ExecuteNonQuery();
                //
                _status = int.Parse(errCodeParam.Value.ToString());
                _message = errMessageParam.Value.ToString();

            }
            catch { throw; }
            // return data.
            return (_status, _message);
        }

        public (int status, string message) UpdateUserPRofile(UserProfileDTO model)
        {
            //UserProfileDTO toreturn = new UserProfileDTO();
            try
            {
                using var dbConnection = new SqlConnection(GetConnectionString());
                dbConnection.Open();
                var sqlCommand = new SqlCommand
                {
                    Connection = dbConnection,
                    CommandText = "spUpdateUserProfile",
                    CommandTimeout = DataConstants.CONNECTION_TIMEOUT,
                    CommandType = CommandType.StoredProcedure
                };

                sqlCommand.Parameters.AddWithValue("@UserId", model.Id);
                sqlCommand.Parameters.AddWithValue("@FirstName", model.FirstName);
                sqlCommand.Parameters.AddWithValue("@LastName", model.LastName);
                sqlCommand.Parameters.AddWithValue("@Mobile", model.Mobile);
                sqlCommand.Parameters.AddWithValue("@Gender", model.Gender);
                sqlCommand.Parameters.AddWithValue("@DOB", model.DOB);
              
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
                int effectedrows = sqlCommand.ExecuteNonQuery();
                //
                _status = int.Parse(errCodeParam.Value.ToString());
                _message = errMessageParam.Value.ToString();

            }
            catch { throw; }
            // return data.
            return (_status, _message);
        }

    }
}
