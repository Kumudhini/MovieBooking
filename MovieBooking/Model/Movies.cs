using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MovieBooking.Model
{
    public class Movies
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? _id { get; private set; }

        [BsonElement("moviename")]
        public string MovieName { get; set; } = String.Empty;

        [BsonElement("totaltickets")]
        public int TotalNumberOfTickets { get; set; }

        [BsonElement("theatrename")]
        public string TheatreName { get; set; } = String.Empty;

        [BsonElement("moviestatus")]
        public string MovieStatus { get; set; } = String.Empty;

        [BsonElement("ticketremaining")]
        public int TicketsRemaining { get; set; }
    }
}
