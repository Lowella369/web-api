using CustomerAPI.Models;
using CustomerAPI.Models.DTO;

namespace CustomerAPI.Repositories
{
    public interface ICustomerRepository
    {
        Task<List<Customer>>GetAllCustomersAsync();
        Task<Customer?> GetCustomerByIdAsync(Guid id);
        Task<Customer> AddCustomerAsync(Customer customer);
        Task<Customer?> UpdateCustomerAsync(Guid id, Customer customer);
        Task<Customer?> DeleteCustomerAsync(Guid id);

    }
}
