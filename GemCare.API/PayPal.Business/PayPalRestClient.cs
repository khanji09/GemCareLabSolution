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
        public static string CaptureJson;
        public static CaptureOrderResponse captureOrderResponse;
        public static async Task<CreateOrderResponse> CreateOrder(CreateOrderRequest createOrderObject)
        {            
            HttpClient client = new HttpClient();
            string orderUrl = String.Concat(_payPalConfiguration.Url, "v2/checkout/orders");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",AccessToken.GetAccessToken());
            string orderGuid = Guid.NewGuid().ToString();
            client.DefaultRequestHeaders.Add("PayPal-Request-Id",orderGuid);
            client.DefaultRequestHeaders.Add("Prefer", "return=representation");
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
                    createOrderResponse.PayPalRequestId = orderGuid;
                    //return createOrderResponse;
                }
                else
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    createOrderResponse = new CreateOrderResponse
                    {
                        Error = JsonConvert.DeserializeObject<PayPalError>(responseContent),
                        id = null,
                        PayPalRequestId = null
                    };
                }
            }
            return createOrderResponse;
            //return null;
        }

        public static async Task<CaptureOrderResponse> CapturePayment(PayPalCapturePaymentRequest capturePaymentRequest)
        {
            HttpClient client = new HttpClient();
            //string orderUrl = String.Concat(_payPalConfiguration.Url, $"v2/checkout/orders/{capturePaymentRequest.Orderid}/capture");
            string orderUrl = string.Format("{0}v2/checkout/orders/{1}/capture", _payPalConfiguration.Url, capturePaymentRequest.Orderid);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken.GetAccessToken());
            
            client.DefaultRequestHeaders.Add("PayPal-Request-Id", capturePaymentRequest.PayPalrequestid);
            client.BaseAddress = new Uri(orderUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));           

            var content = new StringContent("{}", Encoding.UTF8, "application/json");
            using (HttpResponseMessage response = await client.PostAsync(orderUrl, content))
            {
                if (response.IsSuccessStatusCode)
                {
                    var res = await response.Content.ReadAsStringAsync();//<CreateOrderResponse>().Result;
                                                                         // return createOrderResponse;
                    captureOrderResponse = JsonConvert.DeserializeObject<CaptureOrderResponse>(res);                   
                    return captureOrderResponse;
                }
            }

            return null;
        }

        public static async Task<string> AuthorizePayment(AuthorizePaymentRequest authorizePaymentRequest)
        {
            HttpClient client = new HttpClient();
            string orderUrl = String.Concat(_payPalConfiguration.Url, $"v2/checkout/orders/{authorizePaymentRequest.OrderId}/authorize");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken.GetAccessToken());
            string orderGuid = Guid.NewGuid().ToString();
            client.DefaultRequestHeaders.Add("PayPal-Request-Id", authorizePaymentRequest.PayPalRequestId);
            client.BaseAddress = new Uri(orderUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var content = new StringContent("{}", Encoding.UTF8, "application/json");
            using (HttpResponseMessage response = await client.PostAsync(orderUrl, content))
            {
                if (response.IsSuccessStatusCode)
                {
                    var res = await response.Content.ReadAsStringAsync();
                    CaptureJson = JsonConvert.DeserializeObject<string>(res);
                    return CaptureJson;
                }
            }

            return null;
        }
    }
}
