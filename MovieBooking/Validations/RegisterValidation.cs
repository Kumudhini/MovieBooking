using FluentValidation;
using MovieBooking.Model;

namespace MovieBooking.Validations
{
    public class RegisterValidation : AbstractValidator<Register>
    {
        public RegisterValidation()
        {
            RuleFor(x => x.FirstName).NotEmpty().NotNull().WithMessage("FirstName must not be empty or null.");
            RuleFor(x => x.LastName).NotEmpty().NotNull().WithMessage("LastName must not be empty or null.");
            RuleFor(x => x.Email).NotEmpty().NotNull().WithMessage("Email must not be empty or null.");
            RuleFor(x => x.LoginId).NotEmpty().NotNull().WithMessage("LoginId must not be empty or null.");
            RuleFor(x => x.Password).NotEmpty().NotNull().WithMessage("Password must not be empty or null.");
            RuleFor(x => x.ConfirmPassword).NotEmpty().NotNull().WithMessage("ConfirmPassword must not be empty or null.");
            RuleFor(x => x.ContactNumber).NotEmpty().NotNull().WithMessage("ContactNo must not be empty or null.");
        }
    }
}
