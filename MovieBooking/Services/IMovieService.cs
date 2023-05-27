using MovieBooking.Model;

namespace MovieBooking.Services
{
    public interface IMovieService
    {
        public Task<List<Movies>> GetAllMovie();
        public Task<List<Movies>> Search(string moviename);
    }
}
