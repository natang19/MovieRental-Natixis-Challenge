using Microsoft.EntityFrameworkCore;
using MovieRental.Data;

namespace MovieRental.Movie
{
	public class MovieFeatures : IMovieFeatures
	{
		private readonly MovieRentalDbContext _movieRentalDb;
		public MovieFeatures(MovieRentalDbContext movieRentalDb)
		{
			_movieRentalDb = movieRentalDb;
		}
		
		public async Task<Movie> Save(Movie movie)
		{
			_movieRentalDb.Movies.Add(movie);
			var result = await _movieRentalDb.SaveChangesAsync();
			
			return result <= 0 ? throw new InvalidOperationException("Not possible to save Movie") : movie;
		}

		public IQueryable<Movie> GetMovies()
		{
			return _movieRentalDb.Movies.AsNoTracking();
		}
	}
}
