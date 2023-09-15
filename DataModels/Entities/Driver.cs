using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackTest.DataModels
{
    public class Driver
    {
        public Driver()
        {
        }
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }     
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public static Driver Create(string fname,string lname,string email,string phoneNumber) 
        {
            return new Driver
            {
                FirstName=fname,
                LastName=lname,
                Email=email,
                PhoneNumber=phoneNumber
            };
        }

    }
}
