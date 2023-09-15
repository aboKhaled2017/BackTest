using FluentValidation;

namespace Backend.DtoModels
{
    public record BaseDriverReq(string firstName, string lastname, string email, string phoneNumber);
    public abstract class BaseDriverReqValidator<T> : AbstractValidator<T>
        where T: BaseDriverReq
    {
        public BaseDriverReqValidator()
        {
            RuleFor(x => x.firstName)
                .NotEmpty().WithMessage("first name is required")
                .MinimumLength(2).WithMessage("first name at least 2 letters")
                .MaximumLength(50).WithMessage("first name at maximum should be 50 letters")
                .Matches(@"^[A-Za-z][A-Za-z\s\-]*$").WithMessage("not valid first name");

            RuleFor(x => x.lastname)
                .NotEmpty().WithMessage("last name is required")
                .MinimumLength(2).WithMessage("last name at least 2 letters")
                .MaximumLength(50).WithMessage("last name at maximum should be 50 letters")
                .Matches(@"^[A-Za-z][A-Za-z\s\-]*$").WithMessage("not valid last name");

            RuleFor(x => x.email)
                .NotEmpty().WithMessage("email is required")
                .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$").WithMessage("email is not valid");

            RuleFor(x => x.phoneNumber)
                .NotEmpty().WithMessage("phone number is required")
                .Matches(@"^201(0|1|2|5)[0-9]{8}$").WithMessage("phone number is not valid");
        }
    }
}
