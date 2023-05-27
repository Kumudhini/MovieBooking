using MongoDB.Bson;
using MongoDB.Driver;
using MovieBooking.Model;

namespace MovieBooking.Services
{
    public class MovieService:IMovieService
    {
        private IMongoCollection<Movies> _movie;

        public MovieService(IDatabaseSetting setting, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(setting.DatabaseName);
            _movie = database.GetCollection<Movies>(setting.MovieCollectionName);
        }

        public async Task<List<Movies>> GetAllMovie()
        {
            return await _movie.Find(new BsonDocument()).ToListAsync();

        }

        public async Task<List<Movies>> Search(string moviename)
        {

            var filter = Builders<Movies>.Filter.Empty;
            if (!string.IsNullOrEmpty(moviename))
            {
                filter = Builders<Movies>.Filter.Regex(field: "MovieName", moviename);

            }
            return await _movie.Find(filter).ToListAsync();
        }


    }
}
