using Microsoft.AspNetCore.Mvc;

namespace VenueReservation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { message = "API is working", status = "Success" });
        }
    }
}