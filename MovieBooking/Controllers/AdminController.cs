using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Services;

namespace MovieBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0", Deprecated = true)]
    public class AdminController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        public AdminController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("/api/v{version:apiVersion}/{movieName}/moviebooking/update/{ticket}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateTicketStatus(string movieName, int ticket)
        {
            try
            {
                var response = await _ticketService.UpdateTicketStatus(movieName, ticket);
                return Ok(response);
            }
            catch(Exception)
            {
                return BadRequest("Error Occured while updating status!");
            }
            
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("/api/v{version:apiVersion}/moviebooking/bookedtickets")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetBookedTickets()
        {
            try
            {
                var response = await _ticketService.GetBookedTickets();
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest("Error Occured while Getting Data!");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("/api/v{version:apiVersion}/moviebooking/{movieName}/delete/{id}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> DeleteMovie(string movieName, string id)
        {
            try
            {
                var response = await _ticketService.DeleteMovie(movieName, id);
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest("Error Occured while deleting movie!");
            }

        }
    }
}
