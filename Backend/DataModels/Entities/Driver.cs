namespace Backend.DataModels
{
    public class Driver
    {
        public Driver()
        {
        }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public static Driver Create(string fname, string lname, string email, string phoneNumber)
        {
            return new Driver
            {
                FirstName = fname,
                LastName = lname,
                Email = email,
                PhoneNumber = phoneNumber
            };
        }

    }
}
