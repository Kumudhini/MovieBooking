using MovieBooking.Model;

namespace MovieBooking.Services
{
    public interface ITicketService
    {
        public Task<string> AddTickets(string moviename,Tickets tickets);

        public Task<string> UpdateTicketStatus(string movie, int ticket);

        public Task<List<Tickets>> GetBookedTickets();

        public Task<string> DeleteMovie(string movieName, string id);
    }
}
