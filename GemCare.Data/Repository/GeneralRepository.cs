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
    public class GeneralRepository : BaseRepository, IGeneralRepository
    {
        public GeneralRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public (int status, string message, List<SliderImage> images) GetSliderImages(bool isForMobile)
        {
            List<SliderImage> imageList = null;
            DataTable dt = new();
            // check
            try
            {
                {
                    using var dbConnection = new SqlConnection(GetConnectionString());
                    dbConnection.Open();
                    var sqlCommand = new SqlCommand
                    {
                        Connection = dbConnection,
                        CommandText = "spGetSliderImages",
                        CommandTimeout = DataConstants.CONNECTION_TIMEOUT,
                        CommandType = CommandType.StoredProcedure
                    };
                    sqlCommand.Parameters.AddWithValue("@pIsForMobile", isForMobile);
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
                    var sqlAdapter = new SqlDataAdapter(sqlCommand);
                    sqlAdapter.Fill(dt);
                    errorCode = int.Parse(errCodeParam.Value.ToString());
                    errorMessage = errMessageParam.Value.ToString();
                }
                if (errorCode != -1)
                {
                    imageList = new List<SliderImage>(dt.Rows.Count);
                    foreach (DataRow row in dt.Rows)
                    {
                        var tempObj = new SliderImage
                        {
                            ImageUrl = row["ImageUrl"].ToString(),
                            ShortDescription = row["ShortDescription"].ToString()
                        };
                        imageList.Add(tempObj);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return (errorCode, errorMessage, imageList);
        }
    }
}
