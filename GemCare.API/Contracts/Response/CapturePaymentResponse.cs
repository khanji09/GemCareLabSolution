﻿namespace GemCare.API.Contracts.Response
{
    public class CapturePaymentResponse
    {
        public string Transactionid { get; set; }
        public string Chargeid { get; set; }
    }

    public class PaymentMethodCreateResponse //: BaseResponse
    {
        public string Paymentmethodid { get; set; }
    }
}
