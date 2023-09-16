namespace Backend.DtoModels
{
    public record CreateDriverReq : BaseDriverReq
    {
        public CreateDriverReq(string firstName, string lastName, string email, string phoneNumber)
            : base(firstName, lastName, email, phoneNumber)
        {
        }
    }

    public sealed class CreateDriverReqValidator : BaseDriverReqValidator<CreateDriverReq> { }
}
