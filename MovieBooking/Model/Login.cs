using MongoDB.Bson.Serialization.Attributes;

namespace MovieBooking.Model
{
    public class Login
    {
        [BsonElement("loginid")]
        public string LoginId { get; set; } = String.Empty;

        [BsonElement("password")]
        public string Password { get; set; } = String.Empty;
    }
}
