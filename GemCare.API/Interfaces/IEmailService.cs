using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GemCare.API.Interfaces
{
    public interface IEmailService
    {
        bool SendLoginCode(string toEmail, string resetCode);
    }
}
