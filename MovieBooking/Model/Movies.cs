using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MovieBooking.Model
{
    public class Movies
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? _id { get; private set; }

        public string MovieName { get; set; } = String.Empty;

        public int TotalNumberOfTickets { get; set; }

        public string TheatreName { get; set; } = String.Empty;

        public string MovieStatus { get; set; } = String.Empty;

        public int TicketsRemaining { get; set; }
    }
}
