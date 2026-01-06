using Microsoft.EntityFrameworkCore;
using MovieRental.Data;
using MovieRental.PaymentProviders;

namespace MovieRental.Rental
{
	public class RentalFeatures : IRentalFeatures
	{
		private readonly MovieRentalDbContext _movieRentalDb;
		private readonly IPaymentProviderFactory _paymentProviderFactory;
		
		public RentalFeatures(
			MovieRentalDbContext movieRentalDb, 
			IPaymentProviderFactory paymentProviderFactory)
		{
			_movieRentalDb = movieRentalDb;
			_paymentProviderFactory = paymentProviderFactory;
		}
		
		public async Task<Rental> PerformRental(Rental rental)
		{
			var rentalPrice = CalculateRentalPrice(rental);
			await ProcessPaymentAsync(rental.PaymentMethod, rentalPrice);

			await SaveRental(rental);

			return rental;
		}
		
		private static decimal CalculateRentalPrice(Rental rental)
		{
			return rental.DaysRented * rental.Movie!.Price;
		}
		
		private async Task ProcessPaymentAsync(PaymentMethod paymentMethod, decimal rentalPrice)
		{
			var paymentProvider = _paymentProviderFactory.Create(paymentMethod);

			var paymentSucceeded = await paymentProvider.Pay(rentalPrice);
			
			if (!paymentSucceeded)
			{
				throw new InvalidOperationException("Payment failed");
			}
		}
		
		private async Task SaveRental(Rental rental)
		{
			_movieRentalDb.Rentals.Add(rental);

			var rowsAffected = await _movieRentalDb.SaveChangesAsync();
			if (rowsAffected <= 0)
			{
				throw new InvalidOperationException("Not possible to save Rental");
			}
		}

		public IQueryable<Rental> GetRentals()
		{
			return _movieRentalDb.Rentals.AsNoTracking();
		}
	}
}
