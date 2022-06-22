using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GemCare.API.Services
{
    public interface IAddressService
    {
        Task<bool> IsValidPostalCode(string postalCode);
        Task<List<string>> GetPostalCodeAddresses(string postalCode);
    }

    public class AddressService : IAddressService
    {
        //private const string apiKey = "";
        public async Task<bool> IsValidPostalCode(string postalCode)
        {
            string url = $"https://api.postcodes.io/postcodes/{postalCode}/validate";
            try
            {
                using HttpClient httpClient = new()
                {
                    BaseAddress = new Uri(url)
                };
                HttpResponseMessage response = await httpClient.GetAsync(url);
                var jsonString = response.Content.ReadAsStringAsync().Result;
                var model = JsonConvert.DeserializeObject<ValidatePostalCodeResult>(jsonString);
                return model.Result;
            }
            catch
            {
                return false;
            }
        }
        public async Task<List<string>> GetPostalCodeAddresses(string postalCode)
        {
            List<string> addressList = new();
            //List<AddressModel> addressList = new List<AddressModel>();
            try
            {
                JToken listofaddresses = string.Empty;
                string postalApiBaseUrl = "https://api.getaddress.io/find/";
                using HttpClient client = new()
                {
                    BaseAddress = new Uri(postalApiBaseUrl)
                };
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                //HttpResponseMessage response = await client.GetAsync(postalCodeApiUrl);
                HttpResponseMessage response = await client.GetAsync($"{postalCode}?api-key={SiteKeys.PostalCodeApiKey ?? string.Empty}");
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    //var result = JsonConvert.DeserializeObject<CoordinatesModel>(data);
                    addressList = JObject.Parse(data).SelectToken("addresses").Select(s => Regex.Replace(((string)s), @"[\, ]*\, *", "\n", RegexOptions.Multiline))
                        .ToList();
                    // split the List<string>
                    //result.Addresses.ForEach(address =>
                    //{
                    //    var addressArr = address.Split(',');
                    //    addressList.Add(new AddressModel
                    //    {
                    //        Line1 = addressArr[0]?.Trim() ?? string.Empty,
                    //        Line2 = addressArr[1]?.Trim() ?? string.Empty,
                    //        TownCity = addressArr[5]?.Trim() ?? string.Empty
                    //    });
                    //});
                }
            }
            catch
            {
                throw;
            }
            return addressList;
        }
    }

    public class ValidatePostalCodeResult
    {
        public int Status { get; set; }
        public bool Result { get; set; }
    }
}
