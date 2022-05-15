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
    public class ImageSliderRepository : BaseRepository, IImageSliderRepository
    {
        private int _status;
        private string _message;
        public ImageSliderRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public (int status, string message, List<ImageSliderDTO> sliderImages) GetSliderImages(bool isMobile)
        {
           
            List<ImageSliderDTO> toreturn = new List<ImageSliderDTO>();
            try
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

                sqlCommand.Parameters.AddWithValue("@IsMobile", isMobile);
               
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
               
                if(_status>0)
                {
                    foreach(DataRow row in dt.Rows)
                    {
                        toreturn.Add(new ImageSliderDTO() {
                            Id = int.Parse(row["Id"].ToString()),
                            ImageUrl = row["ImageUrl"].ToString(),
                            CreatedOn = DateTime.Parse(row["CreatedOn"].ToString()),
                            IsDeleted = bool.Parse(row["IsDeleted"].ToString()),
                            IsForMobile = bool.Parse(row["IsForMobile"].ToString()),
                            UpdatedOn = DateTime.Parse(row["UpdatedOn"].ToString())
                        });
                    }
                }

            }
            catch { throw; }
            // return data.
            return (_status, _message, toreturn);
        }
    }
}
