using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GemCare.API.Utils;
using GemCare.API.Contracts.Response;
using Stripe;

namespace GemCare.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // cors policy.
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
            //
            services.AddApplicationDependency();
            services.AddDataDependency();
            services.AddControllers()
                .ConfigureApiBehaviorOptions(option =>
                {
                    option.InvalidModelStateResponseFactory = actionContext =>
                    {
                        //var modelState = actionContext.ModelState.Values;
                        //return new BadRequestObjectResult(modelState);
                        return new OkObjectResult(FormatOutput(actionContext));
                    };
                });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Gem Care Lab", Version = "v1.0" });
            });
            
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

           // app.UseHttpsRedirection();

            app.UseRouting();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("../swagger/v1/swagger.json", "GemCare.API v1"));
            // enable cors
            StripeConfiguration.ApiKey = Configuration.GetSection("Stripe")["SecretKey"];
            app.UseCors("CorsPolicy");
            //
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public IBaseResponse FormatOutput(ActionContext context)
        {
            IBaseResponse response = new BaseResponse();
            foreach (var modelStateKey in context.ModelState.Keys)
            {
                var modelStateVal = context.ModelState[modelStateKey];
                foreach (var error in modelStateVal.Errors)
                {
                    response.Message = error.ErrorMessage;
                    break;
                }
            }
            response.Statuscode = System.Net.HttpStatusCode.BadRequest;
            response.Haserror = true;
            return response;
        }
    }
}
