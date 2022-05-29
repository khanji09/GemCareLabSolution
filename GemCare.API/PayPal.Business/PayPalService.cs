using GemCare.API.PayPal.Business.Request;
using GemCare.API.PayPal.Business.Response;
using System.Threading.Tasks;

namespace GemCare.API.PayPal.Business
{
    public class PayPalService : IPayPalService
    {
        private CreateOrderResponse _createOrderResponse;
        private CaptureOrderResponse _captureOrderResponse;
        private string jsonstring;
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

        private CaptureOrderResponse _CapturePayment(PayPalCapturePaymentRequest obj)
        {
            Task.Run(() => PayPalRestClient.CapturePayment(obj)).Wait();
            this._captureOrderResponse = PayPalRestClient.captureOrderResponse;
            return this._captureOrderResponse;
        }
        public CaptureOrderResponse CapturePayment(PayPalCapturePaymentRequest capturePaymentRequest)
        {
            _CapturePayment(capturePaymentRequest);
            return this._captureOrderResponse;
        }
    }
}
