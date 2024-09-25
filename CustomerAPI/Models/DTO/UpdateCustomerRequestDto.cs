namespace CustomerAPI.Models.DTO
{
    public class UpdateCustomerRequestDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
