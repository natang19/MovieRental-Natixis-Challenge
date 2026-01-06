namespace MovieRental.Rental;

public interface IRentalFeatures
{
	Task<Rental> PerformRental(Rental rental);
	IQueryable<Rental> GetRentals();
}