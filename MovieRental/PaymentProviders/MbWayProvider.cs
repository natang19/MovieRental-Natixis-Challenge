namespace MovieRental.PaymentProviders
{
    public class MbWayProvider : IPaymentProvider
    {
        public PaymentMethod Method => PaymentMethod.MbWay;
        
        public Task<bool> Pay(decimal price)
        {
            //ignore this implementation
            return Task.FromResult<bool>(true);
        }
    }
}
