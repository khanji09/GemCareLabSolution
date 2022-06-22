using GemCare.API.Common;
using GemCare.API.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GemCare.API.Services
{
    public class PushService : IPushService
    {
        private readonly IConfiguration _configuration;
        private const string FCM_CONFIG = "FCMConfiguration";
        private string serverKey = string.Empty;
        private string senderId = string.Empty;

        public PushService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public (bool notificationSent, string errMessage) SendBookingNotificationToAdmin(string mTitle, 
            string mBody, string pushToken, string deviceType)
        {
            try
            {
                InitSetting(deviceType);
                // push notification format
                var payload = new
                {
                    to = pushToken,
                    priority = "high",
                    content_available = true,
                    notification = new
                    {
                        body = mBody,
                        title = mTitle
                    }
                };

                // Using Newtonsoft.Json
                //var jsonBody = JsonConvert.SerializeObject(data);
                var jsonBody = JsonConvert.SerializeObject(payload);
                using var httpRequest = new HttpRequestMessage(HttpMethod.Post, AppConstants.FCM_LINK);
                httpRequest.Headers.TryAddWithoutValidation("Authorization", serverKey);
                httpRequest.Headers.TryAddWithoutValidation("Sender", senderId);
                httpRequest.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                using var httpClient = new HttpClient();
                var result = httpClient.Send(httpRequest);

                if (result.IsSuccessStatusCode)
                {
                    var data = result.Content.ReadAsStringAsync().Result;
                    var response = JsonConvert.DeserializeObject<FireBaseResponse>(data);
                    return (1 == response.Success, result.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    // Use result.StatusCode to handle failure
                    // Your custom error handler here
                    //_logger.LogError($"Error sending notification. Status Code: {result.StatusCode}");
                    return (false, result.Content.ReadAsStringAsync().Result);
                }
            }
            catch
            {
                throw;
            }
        }

        private void InitSetting(string devicePlatform)
        {
            // Get the server key from FCM console
            //var serverKey = string.Format("key={0}", "AAAAPgohOrE:APA91bESho4qeDns8trMI4cjS9lte8pLNNP7kz-i2OXZbWFeDNeJzDxnikYu6z4WLSrBUHvw2xFb8N-PtD8UKxmCxnR1Sgdb2RqFnyd-RaDOoQQP6YpP9_jCeGvXtCRitn5bHnaf3nSD");
            // Get the sender id from FCM console
            if (devicePlatform.Equals("android"))
            {
                serverKey = string.Format("key={0}", _configuration.GetSection(FCM_CONFIG)
                    .GetChildren().FirstOrDefault(x => x.Key == "ServerKeyCustomer_android").Value);
                senderId = string.Format("id={0}", _configuration.GetSection(FCM_CONFIG)
                    .GetChildren().FirstOrDefault(x => x.Key == "SenderIdCustomer_android").Value);
            }
            else
            {
                // iOS setting
            }
        }
    }

    public class FireBaseResponse
    {
        public int Success { get; set; }
    }
}
