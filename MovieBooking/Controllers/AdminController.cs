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
        //[HttpGet("/api/v{version:apiVersion}/moviebooking/bookedtickets")]
        [HttpGet(Constants.RoutingConstant.BookedTickets)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetBookedTickets()
        {
            try
            {
                var response = await _ticketService.GetBookedTickets();
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
        //[HttpDelete("/api/v{version:apiVersion}/moviebooking/{movieName}/delete/{id}")]
        [HttpDelete(RoutingConstant.DeleteMovie)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> DeleteMovie(string moviename, string id)
        {
            try
            {
                var response = await _ticketService.DeleteMovie(moviename, id);
                _logger.LogInformation("Movie deleted successfully for the id :{id}",id);
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
