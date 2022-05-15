using GemCare.API.Common;
using GemCare.API.Helper;
using GemCare.API.Interfaces;
using GemCare.Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GemCare.API.Utils
{
    public static class ApplicationDependency
    {
        public static IServiceCollection AddApplicationDependency(this IServiceCollection services)
        {
            services.AddTransient<ITokenGenerator, TokenGenerator>();
            services.AddScoped<IEncryptionDecryptionHelper, EncryptionDecryptionHelper>();
            services.AddTransient<IImageHelper,ImageHelper>();           
            
            return services;
        }
    }
}
