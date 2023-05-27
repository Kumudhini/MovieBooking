using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MovieBooking.Model
{
    public class Tickets
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; private set; }

        [BsonElement("moviename")]
        public string MovieName { get; set; } = String.Empty;

        [BsonElement("theatreName")]
        public string TheatreName { get; set; } = String.Empty;

        [BsonElement("totalticket")]
        public int NumberOfTickets { get; set; }

        [BsonElement("seatNumber")]
        public List<string>? SeatNumber { get; set; }
    }
}
