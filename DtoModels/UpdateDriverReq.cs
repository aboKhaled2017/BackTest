using BackTest.DataModels;

namespace BackTest.DtoModels
{
    public record UpdateDriverReq : BaseDriverReq
    {
        public UpdateDriverReq(string firstName, string lastname, string email, string phoneNumber) 
            : base(firstName, lastname, email, phoneNumber)
        {
        }

        public Driver BindTo(Driver driver)
        {
            driver.FirstName = firstName;
            driver.LastName = lastname;
            driver.PhoneNumber = phoneNumber;
            driver.Email = email;

            return driver;
        }
    }
    public sealed class UpdateDriverReqValidator : BaseDriverReqValidator<UpdateDriverReq> { }
}
