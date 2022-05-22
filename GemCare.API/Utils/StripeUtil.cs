using Stripe;
using System;
using System.Collections.Generic;

namespace GemCare.API.Utils
{

    public class StripeUtil
    {

        public StripeUtil()
        {

        }

        public PaymentIntent CapturePayment(int Amount, string Currency, List<string> PaymentMethodTypes, string CaptureMethod,
            Dictionary<string, string> MetaData, string paymentMethod, string StatementDescriptor, string RecieptEmail)
        {
            PaymentIntent toReturn = new PaymentIntent();
            try
            {
                toReturn = _createPaymentIntent(Amount, Currency, PaymentMethodTypes, CaptureMethod,
                    MetaData, paymentMethod, StatementDescriptor, RecieptEmail);

                if (toReturn.Status.ToLower().Equals("requires_confirmation"))
                {
                    var paymentIntentCaptureOptions = new PaymentIntentCaptureOptions
                    {
                        AmountToCapture = Convert.ToInt64(Amount * 100)
                    };
                    var paymentIntentService = new PaymentIntentService();
                    toReturn = paymentIntentService.Capture(toReturn.Id, paymentIntentCaptureOptions);                    
                }

            }
            catch (Exception ae)
            {
                throw new Exception(ae.Message);
            }
            return toReturn;
        }
        private PaymentIntent _createPaymentIntent(int Amount, string Currency, List<string> PaymentMethodTypes, string CaptureMethod,
            Dictionary<string, string> MetaData, string paymentMethod, string StatementDescriptor, string RecieptEmail)
        {
            PaymentIntent toReturn = new PaymentIntent();
            try
            {
                var paymentIntentCreateOptions = new PaymentIntentCreateOptions
                {
                    Amount = Amount * 100,
                    Currency = Currency,
                    PaymentMethodTypes = PaymentMethodTypes,
                    CaptureMethod = CaptureMethod,
                    Metadata = MetaData,
                    PaymentMethod = paymentMethod,                    
                    StatementDescriptor = StatementDescriptor,
                    ReceiptEmail = RecieptEmail
                };
                var paymentIntentService = new PaymentIntentService();
                toReturn = paymentIntentService.Create(paymentIntentCreateOptions);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return toReturn;
        }
    }
}
