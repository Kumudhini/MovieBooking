using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MovieBooking.Model
{
    public class Register
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? _id { get; private set; }

        [BsonElement("fname")]
        public string FirstName { get; set; } = String.Empty;

        [BsonElement("lname")]
        [BsonRequired]
        public string LastName { get; set; } = String.Empty;

        [BsonElement("email")]
        public string Email { get; set; } = String.Empty;

        [BsonElement("loginid")]
        public string LoginId { get; set; } = String.Empty;

        [BsonElement("password")]
        public string Password { get; set; } = String.Empty;

        [BsonElement("cpassword")]
        public string ConfirmPassword { get; set; } = String.Empty;

        [BsonElement("contactno")]
        public int ContactNumber { get; set; }

        [BsonElement("role")]
        public string Role { get; private set; } = "user";


    }
}
