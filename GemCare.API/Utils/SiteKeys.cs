using GemCare.API.PayPal.Business;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


public static class SiteKeys
{
    private static IConfiguration _configuration;
     static SiteKeys()
    {       
        _configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
    }
    
    public static PayPalConfiguration PayPal
    {
        get
        {
            PayPalConfiguration payPalConfiguration = new PayPalConfiguration();
            payPalConfiguration.ClientId= _configuration.GetSection("PayPal")["ClientID"];
            payPalConfiguration.SecretKey= _configuration.GetSection("PayPal")["SecretKey"];
            bool UseProductionEnviroment = bool.Parse(_configuration.GetSection("PayPal")["UseProductionEnviroment"]);
            if(UseProductionEnviroment)
            {
                payPalConfiguration.Url = _configuration.GetSection("PayPal")["ProductionUrl"];
            }
            else
            {
                payPalConfiguration.Url = _configuration.GetSection("PayPal")["SandboxUrl"];
            }
           return payPalConfiguration;
        }
    } 
}

