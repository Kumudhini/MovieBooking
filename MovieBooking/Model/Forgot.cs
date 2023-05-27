using MongoDB.Bson.Serialization.Attributes;

namespace MovieBooking.Model
{
    public class Forgot
    {
        [BsonElement("email")]
        public string Email { get; set; } = String.Empty;

        [BsonElement("password")]
        public string Password { get; set; } = String.Empty;

        [BsonElement("cpassword")]
        public string ConfirmPassword { get; set; } = String.Empty;
    }
}
