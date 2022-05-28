using GemCare.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemCare.Data.Interfaces
{
    public interface IDashboardRepository
    {
        (int status, string message, JobsSummary summary) GetJobsSummary();
        (int status, string message, List<AppUser> technicians) GetTechnicians();
        (int status, string message, List<AllJobsCountByMWY> jobsCount) GetAllJobsCountByMWY(string filter);
        (int status, string message) TechnicianRegistration(UserBasicInfo basicInfo);
    }
}
