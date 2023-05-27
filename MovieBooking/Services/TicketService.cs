using MongoDB.Driver;
using MovieBooking.Model;

namespace MovieBooking.Services
{
    public class TicketService : ITicketService
    {
        private IMongoCollection<Tickets> _tickets;
        private IMongoCollection<Movies> _movie;
        public TicketService(IDatabaseSetting setting, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(setting.DatabaseName);
            _tickets = database.GetCollection<Tickets>(setting.TicketCollection);
            _movie = database.GetCollection<Movies>(setting.MovieCollectionName);
        }
        public async Task<string> AddTickets(string moviename,Tickets tickets)
        {
            var msg = String.Empty;
            var noOfTicket = await _movie.Find<Movies>(m => m.MovieName == moviename && m.TheatreName == tickets.TheatreName && m.TotalNumberOfTickets != 0).FirstOrDefaultAsync();
            if (noOfTicket == null)
            {
                msg = "Movie or theatre not found.";
                return msg;
            }
            if (tickets.NumberOfTickets > noOfTicket.TotalNumberOfTickets)
            {
                msg = "Insufficient tickets available.";
                return msg;
            }
            await _tickets.InsertOneAsync(tickets);
            var totalnumberoftickets = noOfTicket.TotalNumberOfTickets - tickets.NumberOfTickets;
            var updateDefinition = Builders<Movies>.Update.Set(m => m.TotalNumberOfTickets, totalnumberoftickets);
            await _movie.UpdateOneAsync(m => m._id == noOfTicket._id, updateDefinition);

            msg = "Tickets booked successfully.";
            return msg;

        }

        public async Task<string> UpdateTicketStatus(string movieName, int ticket)
        {
            var movie = await _movie.Find(m => m.MovieName == movieName).SingleOrDefaultAsync();
            string msg = String.Empty;
            if (movie == null)
            {
                msg = "Movie not found";
                return msg;
            }

            if (ticket == 0)
                movie.MovieStatus = "SOLD OUT";
            else
                movie.MovieStatus = "BOOK ASAP";

            _movie.ReplaceOne(m => m.MovieName == movieName, movie);
            msg = "Ticket status updated successfully";
            return msg;

        }
        public async Task<List<Tickets>> GetBookedTickets()
        {
            var tickets = await _tickets.Find(_ => true).ToListAsync();
            return tickets;
        }

        public async Task<string> DeleteMovie(string movieName, string id)
        {
            var result = await _movie.DeleteOneAsync(m => m.MovieName == movieName && m._id == id);
            string msg = String.Empty;
            if (result.DeletedCount == 0)
            {
                msg = "Movie not found";
                return msg;

            }

            msg = "Movie deleted successfully";
            return msg;
        }

    }
}
