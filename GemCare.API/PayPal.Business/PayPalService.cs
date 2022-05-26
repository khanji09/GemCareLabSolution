using GemCare.API.PayPal.Business.Request;
using GemCare.API.PayPal.Business.Response;
using System.Threading.Tasks;

namespace GemCare.API.PayPal.Business
{
    public class PayPalService : IPayPalService
    {
        private CreateOrderResponse _createOrderResponse;
        private CreateOrderResponse _CreateOrderResponse(CreateOrderRequest obj)
        {
            Task.Run(() => PayPalRestClient.CreateOrder(obj)).Wait();
            this._createOrderResponse= PayPalRestClient.createOrderResponse;
            return _createOrderResponse;
        }
        public CreateOrderResponse CreateOrder(CreateOrderRequest createOrderObject)
        {
            _CreateOrderResponse(createOrderObject);
            return this._createOrderResponse;
        }
    }
}
