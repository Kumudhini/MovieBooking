using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Model;
using MovieBooking.Services;

namespace MovieBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieBookingController : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly ITicketService _ticketService;

        public MovieBookingController(IMovieService movieService, ITicketService ticketService)
        {
            _movieService = movieService;
            _ticketService = ticketService;
        }

        [Authorize(Roles = "user")]
        [HttpGet("/api/v{version:apiVersion}/moviebooking/allmovies")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<Movies>> GetAllMovie()
        {
            try
            {
                var data = await _movieService.GetAllMovie();
                return Ok(data);
            }
            catch (Exception)
            {
                return BadRequest("Error Occured while getting data!");
            }
        }

        [Authorize(Roles = "user")]
        [HttpGet("/api/v{version:apiVersion}/moviebooking/movies/search/{moviename}")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<Movies>> Search(string moviename)
        {
            try
            {
                return Ok(await _movieService.Search(moviename));
            }
            catch(Exception)
            {
                return BadRequest("Error Occured while searching movie!");
            }
}

        [Authorize(Roles = "user")]
        [HttpPost("/api/v{version:apiVersion}/moviebooking/{moviename}/add")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> AddTicket(string moviename,Tickets ticket)
        {
            try
            {
                var response = await _ticketService.AddTickets(moviename,ticket);
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest("Error Occured while booking ticket!");
            }
        }
        
    }
}
