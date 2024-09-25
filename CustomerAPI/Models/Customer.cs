namespace CustomerAPI.Models
{
    public class Customer
    {
       
        public string FirstName { get; set; }
        public Guid Id { get; set; }
        public string LastName { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
