using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GemCare.API.PayPal.Business
{
    public class AccessToken
    {
        private static PayPalConfiguration _payPalConfiguration = SiteKeys.PayPal;
        private static PayPalAuthenticationResponse auth;
        private static  async Task<PayPalAuthenticationResponse> _GenerateAccessToken()
        {
            string url = string.Concat(_payPalConfiguration.Url,"v1/oauth2/token");
            
            using (HttpClient http = new HttpClient())
            {
                http.BaseAddress = new Uri(url);
                byte[] bytes = Encoding.GetEncoding("iso-8859-1").GetBytes($"{_payPalConfiguration.ClientId}:{_payPalConfiguration.SecretKey}");

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/v1/oauth2/token");
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(bytes));

                var form = new Dictionary<string, string>
                {
                    ["grant_type"] = "client_credentials"
                };
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                //ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                //ServicePointManager.Expect100Continue = false;

                request.Content = new FormUrlEncodedContent(form);

                HttpResponseMessage response = await http.SendAsync(request);

                string content = await response.Content.ReadAsStringAsync();
                PayPalAuthenticationResponse accessToken = JsonConvert.DeserializeObject<PayPalAuthenticationResponse>(content);
                auth = accessToken;
                auth.bearer_access_token = string.Concat("Bearer ", accessToken.access_token);
                return accessToken;
                
            }
        }

        public static string GetAccessToken()
        {
            Task.Run(() => _GenerateAccessToken()).Wait();
            return auth.access_token;
        }
        public static string GetAccessTokenWithBearer()
        {
            Task.Run(() => _GenerateAccessToken()).Wait();
            return auth.bearer_access_token;
        }

        public static PayPalAuthenticationResponse GetAuthObject()
        {
            Task.Run(() => _GenerateAccessToken()).Wait();
            return auth;
        }

    }
}
