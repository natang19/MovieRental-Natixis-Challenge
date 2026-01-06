namespace MovieRental.PaymentProviders
{
    public class PayPalProvider : IPaymentProvider
    {
        public PaymentMethod Method => PaymentMethod.PayPal;
        
        public Task<bool> Pay(decimal price)
        {
            //ignore this implementation
            return Task.FromResult<bool>(true);
        }
    }
}
