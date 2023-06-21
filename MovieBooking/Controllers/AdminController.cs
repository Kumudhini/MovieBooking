using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Constants;
using MovieBooking.Services;

namespace MovieBooking.Controllers
{
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        private readonly ILogger<AdminController> _logger;
        public AdminController(ITicketService ticketService, ILogger<AdminController> logger)
        {
            _ticketService = ticketService;
            _logger = logger;
        }
        //[Authorize(Roles = "Admin")]
        [HttpPut(Constants.RoutingConstant.UpdateTicketStatus)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateTicketStatus(string movieName, int ticket)
        {
            try
            {
                var response = await _ticketService.UpdateTicketStatus(movieName, ticket);
                return Ok(response);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new
                {
                    StatusCode = Constant.NotFound,
                    Response = Constant.ErrorForUpdateTicketData
                });
            }
            
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet(Constants.RoutingConstant.BookedTickets)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetBookedTickets(string movieName,string theaterName)
        {
            try
            {
                var response = await _ticketService.GetBookedTickets(movieName, theaterName);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new
                {
                    StatusCode = Constant.NotFound,
                    Response = Constant.ErrorForGetData
                }) ;
            }
        }

        //[Authorize(Roles = "Admin")]
        [HttpDelete(RoutingConstant.DeleteMovie)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> DeleteMovie(string moviename, string theatername)
        {
            try
            {
                var response = await _ticketService.DeleteMovie(moviename, theatername);
                _logger.LogInformation("Movie deleted successfully for the Movie:{moviename} in the Theater :{theatername}", moviename,theatername);
                return Ok(new
                {
                    StatusCode = Constant.OkResponse,
                    Response = response,
                });
            }
            catch (Exception ex)
            {
                
                _logger.LogError(ex.Message);
                return BadRequest(new
                {
                    StatusCode = Constant.NotFound,
                    Message = Constant.ErrorForDelete
                }); ;
            }

        }
    }
}
