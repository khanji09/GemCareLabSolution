using GemCare.Data.Interfaces;
using GemCare.Data.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GemCare.API.Utils
{
    public static class DataDependency
    {
        public static IServiceCollection AddDataDependency(this IServiceCollection services)
        {
            services.AddTransient<IAuthenticate, UserAuthentication>();
            services.AddTransient<IGeneralRepository, GeneralRepository>();
            return services;
        }
    }
}
