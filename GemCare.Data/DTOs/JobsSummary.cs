using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemCare.Data.DTOs
{
    public class JobsSummary
    {
        public int UnallocatedJobs { get; set; }
        public int AllocatedJobs { get; set; }
        public int CancelledJobs { get; set; }
        public int CompletedJobs { get; set; }
    }

    public class AllJobsCountByMWY
    {
        public string ReturnName { get; set; }
        //public JobsSummary JobsCount { get; set; }
        public int UnallocatedJobs { get; set; }
        public int AllocatedJobs { get; set; }
        public int CancelledJobs { get; set; }
        public int CompletedJobs { get; set; }
    }
}
