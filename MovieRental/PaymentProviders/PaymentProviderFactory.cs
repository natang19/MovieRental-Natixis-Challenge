namespace MovieRental.PaymentProviders;

public interface IPaymentProviderFactory
{
    IPaymentProvider Create(PaymentMethod paymentMethod);
}

public class PaymentProviderFactory : IPaymentProviderFactory
{
    private readonly IEnumerable<IPaymentProvider> _providers;

    public PaymentProviderFactory(IEnumerable<IPaymentProvider> providers)
    {
        _providers = providers;
    }

    public IPaymentProvider Create(PaymentMethod paymentMethod)
    {
        var provider = _providers.FirstOrDefault(p => p.Method == paymentMethod);

        if (provider is null)
        {
            throw new InvalidOperationException($"{paymentMethod} not available");
        }

        return provider;
    }
}