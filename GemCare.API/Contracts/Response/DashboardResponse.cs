using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GemCare.API.Contracts.Response
{
    public class JobsSummaryResponse
    {
        public int Pendingjobs { get; set; }
        public int Allocatedjobs { get; set; }
        public int Cancelledjobs { get; set; }
        public int Completedjobs { get; set; }
    }

    public class DashboardAllJobsResponse
    {
        public string Name { get; set; }
        public JobsSummaryResponse Summary { get; set; }
    }
}
