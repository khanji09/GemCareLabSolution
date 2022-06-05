using GemCare.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemCare.Data.Interfaces
{
    public interface IValuationRepository
    {
        (int status, string message,int ValuationId) AddValuation(ValuationDTO model);
        (int status, string message, List<AdminValuationDTO>) GetValuationsAdmin();
        (int status, string message, List<AdminValuationDTO>) GetValuationsRequests(int userid, bool isTechnician = false);
    }
}
