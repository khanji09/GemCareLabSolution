using GemCare.API.PayPal.Business.Request;
using GemCare.API.PayPal.Business.Response;

namespace GemCare.API.PayPal.Business
{
    public interface IPayPalService
    {
        CreateOrderResponse CreateOrder(CreateOrderRequest createOrderObject);
        CaptureOrderResponse CapturePayment(PayPalCapturePaymentRequest capturePaymentRequest);
    }
}
