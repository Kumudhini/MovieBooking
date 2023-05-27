using MongoDB.Bson.Serialization.Attributes;

namespace MovieBooking.Model
{
    public class ForgetPassword
    {
        [BsonElement("password")]
        public string Password { get; set; } = String.Empty;

        [BsonElement("cpassword")]
        public string ConfirmPassword { get; set; } = String.Empty;
    }
}
