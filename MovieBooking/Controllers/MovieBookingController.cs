using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Constants;
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
        private readonly ILogger<MovieBookingController> _logger;

        public MovieBookingController(IMovieService movieService, ITicketService ticketService, ILogger<MovieBookingController> logger)
        {
            _movieService = movieService;
            _ticketService = ticketService;
            _logger = logger;
        }

        //[Authorize(Roles = "user")]
        [HttpGet(RoutingConstant.ViewAllMovies)]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<Movies>> GetAllMovie()
        {
            try
            {
                var data = await _movieService.GetAllMovie();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new
                {
                    StatusCode = Constant.NotFound, Response = Constant.ErrorForGetData
                });
            }
        }

        //[Authorize(Roles = "user")]
        [HttpGet(RoutingConstant.SearchByMovieName)]
        [MapToApiVersion("1.0")]
        public async Task<List<Movies>> Search(string moviename)
        {
            try
            {
                return await _movieService.Search(moviename);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
}

        // [Authorize(Roles = "user")]
        [HttpPost(RoutingConstant.BookTicket)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> AddTicket(string moviename,int ticket)
        {
            try
            {
                var response = await _ticketService.AddTickets(moviename,ticket);
                return Ok(new
                {
                    StatusCode = 200,
                    Response = response,
                    Message = "Ticket Booked Successfully",
                });
            }
            catch (Exception)
            {
                return BadRequest(new
                {
                    StatusCode = Constant.NotFound, Response = Constant.ErrorForBookTicket
                });
            }
        }
        [HttpPost("/api/addmovie")]
        public async Task<IActionResult> AddMovie(Movies movies)
        {
            var respose = await _ticketService.AddMovie(movies);
            return Ok(respose);
        }
        
    }
}
