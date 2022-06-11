using GemCare.Data.Common;
using GemCare.Data.DTOs;
using GemCare.Data.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;

namespace GemCare.Data.Repository
{
    public class UserAuthentication : BaseRepository, IAuthenticate
    {
        //private int errorCode;
        //private string errorMessage;
        public UserAuthentication(IConfiguration configuration) : base(configuration)
        {
        }
        public (int status, string message, AppUser user) CustomerLogin(string email, string password)
        {
            DataTable dt = new();
            AppUser appUser = null;
            try
            {
                //var (isValid, message) = IsValidCredentials(email, password, true);
                //if (isValid)
                //{ 
                //}
                //else
                //{
                //    errorCode = -1;
                //    errorMessage = message;
                //}
                using var dbConnection = new SqlConnection(GetConnectionString());
                dbConnection.Open();
                var sqlCommand = new SqlCommand
                {
                    Connection = dbConnection,
                    CommandText = "spCustomerLogin",
                    CommandTimeout = DataConstants.CONNECTION_TIMEOUT,
                    CommandType = CommandType.StoredProcedure
                };

                sqlCommand.Parameters.AddWithValue("@pEmail", email);

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
                var sqlAdapter = new SqlDataAdapter(sqlCommand);
                sqlAdapter.Fill(dt);
                //
                errorCode = int.Parse(errCodeParam.Value.ToString());
                errorMessage = errMessageParam.Value.ToString();
                if (errorCode == 1)
                {
                    DataRow row = dt.Rows[0];
                    //
                    var isValidPassword = HashPassword.VerifyPasswordHash(password, (byte[])row["PasswordHash"], (byte[])row["PasswordSalt"]);
                    if (isValidPassword)
                    {
                        appUser = new AppUser
                        {
                            Id = int.Parse(row["UserId"].ToString()),
                            FirstName = row["FirstName"].ToString(),
                            LastName = row["LastName"].ToString(),
                            UserRole = row["UserRole"].ToString(),
                            ImagePath = row["ImagePath"].ToString(),
                            SMSOTP = int.Parse(row["SMSOTP"].ToString()),
                            EmailCode = int.Parse(row["EmailCode"].ToString()),
                            Email = row["Email"].ToString()
                        };
                    }
                    else
                    {
                        errorCode = -1;
                        errorMessage = "Incorrect credentials provided. Please try again.";
                    }
                }
            }
            catch
            {
                throw;
            }

            return (errorCode, errorMessage, appUser);
        }

        public (int status, string message) CustomerRegistration(CustomerBasicInfo basicInfo)
        {
            try
            {
                HashPassword.CreatePasswordHash(basicInfo.Password, out byte[] passwordHash, out byte[] passwordSalt);
                using var dbConnection = new SqlConnection(GetConnectionString());
                dbConnection.Open();
                var sqlCommand = new SqlCommand
                {
                    Connection = dbConnection,
                    CommandText = "spCustomerSignup",
                    CommandTimeout = DataConstants.CONNECTION_TIMEOUT,
                    CommandType = CommandType.StoredProcedure
                };

                sqlCommand.Parameters.AddWithValue("@pFirstName", basicInfo.FirstName);
                sqlCommand.Parameters.AddWithValue("@pLastName", basicInfo.LastName);
                sqlCommand.Parameters.AddWithValue("@pEmail", basicInfo.Email);
                sqlCommand.Parameters.AddWithValue("@pPasswordHash", passwordHash);
                sqlCommand.Parameters.AddWithValue("@pPasswordSalt", passwordSalt);
                sqlCommand.Parameters.AddWithValue("@pMobile", basicInfo.Mobile);
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

        public (int status, string message) SignOut(int userId, string deviceId)
        {
            throw new NotImplementedException();
        }

        private static (bool isValid, string message) IsValidCredentials(string email, string password, bool isTutor)
        {
            bool isValidUser = false;
            string errMessage = string.Empty;
            DataTable dt = new();
            try
            {
                using var dbConnection = new SqlConnection(GetConnectionString());
                dbConnection.Open();
                var sqlCommand = new SqlCommand
                {
                    Connection = dbConnection,
                    CommandText = "spGetUserPasswordHash",
                    CommandTimeout = DataConstants.CONNECTION_TIMEOUT,
                    CommandType = CommandType.StoredProcedure
                };

                sqlCommand.Parameters.AddWithValue("@pEmail", email);
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
                var sqlAdapter = new SqlDataAdapter(sqlCommand);
                sqlAdapter.Fill(dt);
                //
                int errCode = int.Parse(errCodeParam.Value.ToString());
                errMessage = errMessageParam.Value.ToString();
                if (errCode == 1)
                {
                    DataRow row = dt.Rows[0];
                    byte[] PasswordHash = (byte[])row["PasswordHash"];
                    byte[] PasswordSalt = (byte[])row["PasswordSalt"];
                    if (HashPassword.VerifyPasswordHash(password, PasswordHash, PasswordSalt))
                    {
                        isValidUser = true;
                    }
                    else
                    {
                        // Update provider account lock status ??
                        errMessage = "Email Address is incorrect. Please try again.";
                    }
                }
            }
            catch { throw; }
            // return data.
            return (isValidUser, errMessage);
        }

        public (int status, string message, AppUser user) VerifyEmailLoginCode(EmailLoginCodeDTO model)
        {
            DataTable dt = new();
            AppUser appUser = null;
            try
            {
                using var dbConnection = new SqlConnection(GetConnectionString());
                dbConnection.Open();
                var sqlCommand = new SqlCommand
                {
                    Connection = dbConnection,
                    CommandText = "spVerifyEmailCodeLogin",
                    CommandTimeout = DataConstants.CONNECTION_TIMEOUT,
                    CommandType = CommandType.StoredProcedure
                };

                sqlCommand.Parameters.AddWithValue("@UserId", model.UserId);
                sqlCommand.Parameters.AddWithValue("@EmailCode", model.EmailCode);


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
                var sqlAdapter = new SqlDataAdapter(sqlCommand);
                sqlAdapter.Fill(dt);
                //
                errorCode = int.Parse(errCodeParam.Value.ToString());
                errorMessage = errMessageParam.Value.ToString();
                if (errorCode == 1)
                {
                    if (dt.Rows.Count > 0)
                    {
                        DataRow row = dt.Rows[0];
                        appUser = new AppUser
                        {
                            Id = int.Parse(row["UserId"].ToString()),
                            FirstName = row["FirstName"].ToString(),
                            LastName = row["LastName"].ToString(),
                            UserRole = row["UserRole"].ToString(),
                            ImagePath = row["ImagePath"].ToString(),
                            Email = row["Email"].ToString()
                        };
                    }
                }
            }
            catch
            {
                throw;
            }

            return (errorCode, errorMessage, appUser);
        }

        public (int status, string message, AppUser user) AdminLogin(string email, string password)
        {
            DataTable dt = new();
            AppUser appUser = null;
            try
            {
                using var dbConnection = new SqlConnection(GetConnectionString());
                dbConnection.Open();
                var sqlCommand = new SqlCommand
                {
                    Connection = dbConnection,
                    CommandText = "spAdminLogin",
                    CommandTimeout = DataConstants.CONNECTION_TIMEOUT,
                    CommandType = CommandType.StoredProcedure
                };

                sqlCommand.Parameters.AddWithValue("@pEmail", email);

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
                var sqlAdapter = new SqlDataAdapter(sqlCommand);
                sqlAdapter.Fill(dt);
                //
                errorCode = int.Parse(errCodeParam.Value.ToString());
                errorMessage = errMessageParam.Value.ToString();
                if (errorCode == 1)
                {
                    DataRow row = dt.Rows[0];
                    //
                    var isValidPassword = HashPassword.VerifyPasswordHash(password, (byte[])row["PasswordHash"], (byte[])row["PasswordSalt"]);
                    if (isValidPassword)
                    {
                        appUser = new AppUser
                        {
                            Id = int.Parse(row["UserId"].ToString()),
                            FirstName = row["FirstName"].ToString(),
                            LastName = row["LastName"].ToString(),
                            UserRole = row["UserRole"].ToString(),
                            ImagePath = row["ImagePath"].ToString(),
                            SMSOTP = int.Parse(row["SMSOTP"].ToString()),
                            EmailCode = int.Parse(row["EmailCode"].ToString())
                        };
                    }
                    else
                    {
                        errorCode = -1;
                        errorMessage = "Incorrect login credentials provided. Please try again.";
                    }
                }
            }
            catch
            {
                throw;
            }

            return (errorCode, errorMessage, appUser);
        }

        public (int status, string message, AppUser user) TechnicianLogin(string email, string password)
        {
            DataTable dt = new();
            AppUser appUser = null;
            try
            {
                using var dbConnection = new SqlConnection(GetConnectionString());
                dbConnection.Open();
                var sqlCommand = new SqlCommand
                {
                    Connection = dbConnection,
                    CommandText = "spTechnicianLogin",
                    CommandTimeout = DataConstants.CONNECTION_TIMEOUT,
                    CommandType = CommandType.StoredProcedure
                };

                sqlCommand.Parameters.AddWithValue("@pEmail", email);

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
                var sqlAdapter = new SqlDataAdapter(sqlCommand);
                sqlAdapter.Fill(dt);
                //
                errorCode = int.Parse(errCodeParam.Value.ToString());
                errorMessage = errMessageParam.Value.ToString();
                if (errorCode == 1)
                {
                    DataRow row = dt.Rows[0];
                    //
                    var isValidPassword = HashPassword.VerifyPasswordHash(password, (byte[])row["PasswordHash"], (byte[])row["PasswordSalt"]);
                    if (isValidPassword)
                    {
                        appUser = new AppUser
                        {
                            Id = int.Parse(row["UserId"].ToString()),
                            FirstName = row["FirstName"].ToString(),
                            LastName = row["LastName"].ToString(),
                            UserRole = row["UserRole"].ToString(),
                            ImagePath = row["ImagePath"].ToString(),
                            SMSOTP = int.Parse(row["SMSOTP"].ToString()),
                            EmailCode = int.Parse(row["EmailCode"].ToString())
                        };
                    }
                    else
                    {
                        errorCode = -1;
                        errorMessage = "Incorrect login credentials provided. Please try again.";
                    }
                }
            }
            catch
            {
                throw;
            }

            return (errorCode, errorMessage, appUser);
        }
    }
}
