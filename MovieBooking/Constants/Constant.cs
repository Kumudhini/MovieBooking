namespace MovieBooking.Constants
{
    public class Constant
    {
        public const string EmailLoginNotFound = "EmailId or LoginId already Present";
        public const string PasswordMisMatch = "Both password and confirm password must be same";
        public const string LoginIdPasswordRequired = "Login Id and Password Required";
        public const string LoginSuccessfull = "User Login Successfully";
        public const string LoginInvalid = "LoginId Or Password Invalid";
        public const string BookTicket = "Ticket Booked Successfully";
        public const string DeleteMovie = "Movie Deleted Successfully";
        public const string UpdateTicket = "Ticket Status Updated Successfully";
        public const string UpdateTicketError = "Movie is not available";
        public const int OkResponse = 200;
        public const int NotFound = 400;
        public const string InvalidCredential = "Invalid Credentials!";
        public const string ErrorOccurForPasswordUpdate = "Error Occured while updating password!";
        public const string ErrorOccureLogin = "Error Occured while login!";
        public const string PasswordResetSuccess = "Password reset successfully!";
        public const string ErrorForDelete = "Error Occured while deleting movie!";
        public const string ErrorForGetData = "Error Occured while Getting Data!";
        public const string ErrorForUpdateTicketData = "Error Occured while updating status!";
        public const string ErrorForSearchMovie = "Error Occured while searching movie!";
        public const string ErrorForBookTicket = "Error Occured while booking ticket!";
    }
    public class RoutingConstant
    {
        public const string ApiVersion = "/api/v{version:apiVersion}/moviebooking";
        public const string Register = ApiVersion + "/register";
        public const string Login = ApiVersion + "/login";
        public const string ForgetPassword = ApiVersion + "/{username}/forgetpassword";
        public const string ViewAllMovies = ApiVersion + "/all";
        public const string SearchByMovieName = ApiVersion + "/search/{moviename}";
        public const string BookTicket = ApiVersion + "/{moviename}/add/{ticket}";
        public const string UpdateTicketStatus = ApiVersion + "/{movieName}/update/{ticket}";
        public const string DeleteMovie = ApiVersion + "/{moviename}/delete/{theatername}";
        public const string BookedTickets = ApiVersion + "/moviebooking/bookedtickets";
    }
}
