using GemCare.API.PayPal.Business.Request;
using GemCare.API.PayPal.Business.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GemCare.API.PayPal.Business
{
    public class PayPalRestClient
    {
        private static PayPalConfiguration _payPalConfiguration = SiteKeys.PayPal;
        public static CreateOrderResponse createOrderResponse;
        public static async Task<CreateOrderResponse> CreateOrder(CreateOrderRequest createOrderObject)
        {            
            HttpClient client = new HttpClient();
            string orderUrl = String.Concat(_payPalConfiguration.Url, "v2/checkout/orders");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",AccessToken.GetAccessToken());
            client.DefaultRequestHeaders.Add("PayPal-Request-Id",Guid.NewGuid().ToString());
            client.BaseAddress = new Uri(orderUrl);          
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var jobj = JObject.FromObject(createOrderObject) ;
           // string obj = jobj.ToString();

            var content = new StringContent(jobj.ToString(), Encoding.UTF8, "application/json");
            using (HttpResponseMessage response = await client.PostAsync(orderUrl, content))
            {
                if (response.IsSuccessStatusCode)
                {
                    var res = await response.Content.ReadAsStringAsync();//<CreateOrderResponse>().Result;
                                                                         // return createOrderResponse;
                    createOrderResponse = JsonConvert.DeserializeObject<CreateOrderResponse>(res);
                    return createOrderResponse;
                }
            }

            return null;
        }

        //public static async Task<T> Post(JObject jobject, string url, string mediaType = null)
        //{
           
        //    HttpClient client = new HttpClient();

        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken.GetAccessToken());

        //    client.BaseAddress = new Uri(url);
        //    var content = new StringContent(jobject.ToString(), Encoding.UTF8, (mediaType == null) ? "application/json" : mediaType);

        //    using (HttpResponseMessage response = await client.PostAsync(url, content))
        //    {
        //        if (response.IsSuccessStatusCode)
        //        {
        //            return response.Content.ReadAsAsync<T>().Result;
        //        }
        //    }

        //    return null;
        //}

    }
}
