namespace CustomerAPI.Models.DTO
{
    public class AddCustomerRequestDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
