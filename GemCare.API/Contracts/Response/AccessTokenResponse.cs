using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GemCare.API.Contracts.Response
{
    public class AccessTokenResponse : BaseResponse
    {
        public string Paypalaccesstoken { get; set; }
        public AccessTokenResponse()
        {
            Statuscode = System.Net.HttpStatusCode.ExpectationFailed;
            Message = "token not created";
            Paypalaccesstoken = string.Empty;
            Haserror = true;
        }
    }
}
