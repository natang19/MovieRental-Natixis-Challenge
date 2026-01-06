using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using MovieRental.Movie;

namespace MovieRental.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {

        private readonly IMovieFeatures _features;

        public MovieController(IMovieFeatures features)
        {
            _features = features;
        }

        [HttpGet]
        [EnableQuery]
        public ActionResult<IReadOnlyList<Movie.Movie>> Get()
        {
	        return Ok(_features.GetMovies());
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Movie.Movie movie)
        {
            await _features.Save(movie);
	        return Created();
        }
    }
}
