using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using MovieRental.Rental;

namespace MovieRental.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RentalController : ControllerBase
    {
        private readonly IRentalFeatures _features;

        public RentalController(IRentalFeatures features)
        {
            _features = features;
        }

        [HttpGet]
        [EnableQuery]
        public ActionResult<IReadOnlyList<Rental.Rental>> Get()
        {
            return Ok(_features.GetRentals());
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Rental.Rental rental)
        {
            await _features.PerformRental(rental);
	        return Created();
        }
	}
}
