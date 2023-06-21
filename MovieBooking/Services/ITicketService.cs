using MovieBooking.Model;

namespace MovieBooking.Services
{
    public interface ITicketService
    {
        public Task<Tickets> AddTickets(string moviename,int tickets);

        public Task<string> UpdateTicketStatus(string movie, int ticket);

        public Task<List<Tickets>> GetBookedTickets(string moviename, string theatername);

        public Task<string> DeleteMovie(string movieName, string id);

        public Task<Movies> AddMovie(Movies movies);
    }
}
