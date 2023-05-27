using MongoDB.Driver;
using MovieBooking.Model;

namespace MovieBooking.Services
{
    public class UserService : IUserService
    {
        private IMongoCollection<Register> _movie;

        public UserService(IDatabaseSetting setting, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(setting.DatabaseName);
            _movie = database.GetCollection<Register>(setting.CollectionName);
        }
        public async Task<Register> Create(Register register)
        {
            await _movie.InsertOneAsync(register);
            return register;
        }

        public bool UniqueCheck(string email, string loginid)
        {
            var res = _movie.Find<Register>(user => user.Email == email || user.LoginId == loginid).FirstOrDefault();
            if(res == null)
            {
                return true;
            }
            return false;
        }

        public async Task<Register> Login(Login login)
        {
            var response = await _movie.Find<Register>(user => user.LoginId == login.LoginId && user.Password == login.Password).FirstOrDefaultAsync();
            return response;
        }

        public async Task<string> ForgotPassword(string email, ForgetPassword forget)
        {
            FilterDefinition<Register> filter = Builders<Register>.Filter.Eq("Email", email);
            if (filter == null)
            {
              string  msg = Constants.Constant.ErrorOccurForPasswordUpdate;
                return msg;
            }
            else
            {
                UpdateDefinition<Register> update = Builders<Register>.Update.Set("Password", forget.Password).Set("ConfirmPassword",forget.ConfirmPassword);
                await _movie.UpdateOneAsync(filter, update);
               string msg = Constants.Constant.PasswordResetSuccess;
                return msg;
            }
        }
    }
}
