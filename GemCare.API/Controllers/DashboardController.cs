using GemCare.API.Common;
using GemCare.API.Contracts.Response;
using GemCare.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GemCare.API.Controllers
{
    public class DashboardController : BaseApiController
    {
        private readonly IDashboardRepository _dashboardRepository;
        public DashboardController(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        [HttpGet("jobsummary")]
        public IActionResult GetJobsSummary()
        {
            var response = new SingleResponse<JobsSummaryResponse>();
            if (IsValidBearerRequest)
            {
                try
                {
                    var (status, message, summary) = _dashboardRepository.GetJobsSummary();
                    response.Statuscode = 1 == status ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.NotFound;
                    response.Message = message;
                    if(summary != null)
                    {
                        response.Result = new()
                        {
                            Unallocatedjobs = summary.UnallocatedJobs,
                            Allocatedjobs = summary.AllocatedJobs,
                            Completedjobs = summary.CompletedJobs,
                            Cancelledjobs = summary.CancelledJobs
                        };
                    }
                }
                catch(Exception ex)
                {
                    response.ToHttpExceptionResponse(ex.Message);
                }
            }
            else
            {
                response.ToHttpForbiddenResponse(AppConstants.BEARER_ERRMESSAGE);
            }
            return Ok(response);
        }

        [HttpGet("alljobs")]
        public IActionResult AllJobsGraphData(string filter)
        {
            IListResponse<DashboardAllJobsResponse> response = new ListResponse<DashboardAllJobsResponse>();
            if (IsValidBearerRequest)
            {
                try
                {
                    var (status, message, jobsCount) = _dashboardRepository.GetAllJobsCountByMWY(filter);
                    response.Statuscode = 1 == status ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.NotFound;
                    response.Message = message;
                    if (jobsCount?.Count > 0)
                    {
                        response.Result = new List<DashboardAllJobsResponse>();
                        jobsCount.ForEach(item =>
                        {
                            response.Result.Add(new DashboardAllJobsResponse
                            {
                                Name = item.ReturnName,
                                Allocatedjobs = item.AllocatedJobs,
                                Unallocatedjobs = item.UnallocatedJobs,
                                Completedjobs = item.CompletedJobs,
                                Cancelledjobs = item.CancelledJobs
                            });
                        });
                    }
                }
                catch (Exception ex)
                {
                    response.ToHttpExceptionResponse(ex.Message);
                }
            }
            else
            {
                response.ToHttpForbiddenResponse(AppConstants.BEARER_ERRMESSAGE);
            }
            return Ok(response);
        }
    }
}
