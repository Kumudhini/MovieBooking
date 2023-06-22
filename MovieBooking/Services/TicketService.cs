using AspNetCore.Identity.Mongo.Mongo;
using Microsoft.Win32;
using MongoDB.Driver;
using MovieBooking.Model;
using System;
using System.Net.Sockets;

namespace MovieBooking.Services
{
    public class TicketService : ITicketService
    {
        private IMongoCollection<Tickets> _tickets;
        private IMongoCollection<Movies> _movie;
        private ILogger<TicketService> _logger;
        public TicketService(IDatabaseSetting setting, IMongoClient mongoClient, ILogger<TicketService> logger)
        {
            var database = mongoClient.GetDatabase(setting.DatabaseName);
            _tickets = database.GetCollection<Tickets>(setting.TicketCollection);
            _movie = database.GetCollection<Movies>(setting.MovieCollectionName);
            _logger = logger;
        }
        public async Task<Tickets> AddTickets(string moviename,int tickets)
        {
            _logger.LogInformation("Ticket Booking Process started for the Movie :{name}",moviename);
            var msg = String.Empty;
            var noOfTicket = await _movie.Find<Movies>(m => m.MovieName == moviename && m.TotalNumberOfTickets != 0).FirstOrDefaultAsync();

            if (noOfTicket == null)
            {
                _logger.LogError("Movie or theatre not found.");
                return null;
            }
            if (tickets > noOfTicket.TicketsRemaining)
            {
                _logger.LogError("Insufficient tickets available.");
                return null;
            }
            var totalTicketRemaining = noOfTicket.TicketsRemaining - tickets;
            if (totalTicketRemaining < 10)
                noOfTicket.MovieStatus = "SOLD OUT";
            else
                noOfTicket.MovieStatus = "BOOK ASAP";

            _movie.ReplaceOne(m => m.MovieName == moviename, noOfTicket);
            var seatNumbers = new List<string>();
            var ticketList = _tickets.Find<Tickets>(m => m.MovieName == moviename).ToList();
            int totalTicket = ticketList.Sum(x => x.NumberOfTickets);
            var lastSeatNumber = totalTicket + 1;
            for (int i = 0; i < tickets; i++)
            {
                var seatNumber = lastSeatNumber + i;
                seatNumbers.Add(seatNumber.ToString());

            }
            Tickets ticketBook = new Tickets
            {
                MovieName = moviename,
                TheatreName = noOfTicket.TheatreName,
                NumberOfTickets = tickets,
                SeatNumber = seatNumbers
            };
            await _tickets.InsertOneAsync(ticketBook);
            _logger.LogInformation("Movie Booked Successfully");
            var totalnumberoftickets = noOfTicket.TicketsRemaining - tickets;
            var updateDefinition = Builders<Movies>.Update.Set(m => m.TicketsRemaining, totalnumberoftickets);
            await _movie.UpdateOneAsync(m => m._id == noOfTicket._id, updateDefinition);

            return ticketBook;

        }

        public async Task<Movies> AddMovie(Movies movies)
        {
            await _movie.InsertOneAsync(movies);
            return movies;
        }
        public async Task<string> UpdateTicketStatus(string movieName, int ticket)
        {
            string msg = String.Empty;
            try
            {
                var movie = await _movie.Find(m => m.MovieName == movieName).SingleOrDefaultAsync();

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
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return msg;
        }
        public async Task<List<Tickets>> GetBookedTickets(string moviename,string theatername)
        {
            var filter = Builders<Tickets>.Filter.Empty;
            if (!string.IsNullOrEmpty(moviename) && !string.IsNullOrEmpty(moviename))
            {
                filter = Builders<Tickets>.Filter.Regex(field: "MovieName", moviename) & Builders<Tickets>.Filter.Regex(field: "theatreName", theatername);

            }
            return await _tickets.Find(filter).ToListAsync();
        }

        public async Task<string> DeleteMovie(string movieName, string id)
        {
            string msg = String.Empty;
            try
            {
                var result = await _movie.DeleteOneAsync(m => m.MovieName == movieName && m.TheatreName == id);
                
                if (result.DeletedCount == 0)
                {
                    msg = "Movie not found";
                    return msg;

                }
                msg = "Movie deleted successfully";
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            _logger.LogInformation("Movie deleted successfully for the movie :{movie}",movieName);
            return msg;
        }

    }
}
