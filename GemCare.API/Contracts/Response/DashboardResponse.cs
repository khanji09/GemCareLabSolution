using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GemCare.API.Contracts.Response
{
    public class JobsSummaryResponse
    {
        public int Unallocatedjobs { get; set; }
        public int Allocatedjobs { get; set; }
        public int Cancelledjobs { get; set; }
        public int Completedjobs { get; set; }
    }

    public class DashboardAllJobsResponse
    {
        public string Name { get; set; }
        public JobsSummaryResponse Summary { get; set; }
    }

    public class TechnicianResponse
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Mobile { get; set; }
    }
}
