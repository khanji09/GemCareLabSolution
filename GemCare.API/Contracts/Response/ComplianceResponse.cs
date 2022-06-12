using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GemCare.API.Contracts.Response
{
    public class UserAgreementResponse : BaseResponse
    {
        public string Agreementtext { get; set; }
    }
}
