using MovieBooking.Model;

namespace MovieBooking.Services
{
    public interface IAuthService
    {
       public Task<Register> Create(Register register);
       public bool UniqueCheck(string email, string loginid);
       public Task<Register> Login(Login login);
       public Task<string> ForgotPassword(string email, ForgetPassword forget);
    }
}
