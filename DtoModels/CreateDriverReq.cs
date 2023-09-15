using FluentValidation;

namespace BackTest.DtoModels
{
    public record CreateDriverReq : BaseDriverReq
    {
        public CreateDriverReq(string firstName, string lastname, string email, string phoneNumber) 
            : base(firstName, lastname, email, phoneNumber)
        {
        }
    }

    public sealed class CreateDriverReqValidator : BaseDriverReqValidator<CreateDriverReq> { }
}
