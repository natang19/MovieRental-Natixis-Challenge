namespace MovieRental.PaymentProviders;

public interface IPaymentProvider
{
    PaymentMethod Method { get; }
    Task<bool> Pay(decimal price);
}