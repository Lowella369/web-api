using CustomerAPI.Data;
using CustomerAPI.Models;
using CustomerAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace CustomerAPI.Repositories
{
    public class SQLCustomerRepository : ICustomerRepository
    {
        private readonly CustomerDbContext _dbContext;
        public SQLCustomerRepository(CustomerDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<Customer> AddCustomerAsync(Customer customer)
        {
            await _dbContext.Customers.AddAsync(customer); 
            await _dbContext.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer> DeleteCustomerAsync(Guid id)
        {
            var existingCustomer = await _dbContext.Customers.FirstOrDefaultAsync(x => x.Id == id);
            if(existingCustomer == null)
            {
                return null;
            }

            _dbContext.Customers.Remove(existingCustomer);
            await _dbContext.SaveChangesAsync();

            return existingCustomer;
        }

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            return await _dbContext.Customers.ToListAsync();
        }
        public async Task<Customer> GetCustomerByIdAsync(Guid id)
        {
            return await _dbContext.Customers.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Customer?> UpdateCustomerAsync(Guid id, Customer customer)
        {
            var existingCustomer = await _dbContext.Customers.FirstOrDefaultAsync(x => x.Id == id);
            if (existingCustomer == null)
            {
                return null;
            }

            existingCustomer.FirstName = customer.FirstName;
            existingCustomer.LastName = customer.LastName;
            existingCustomer.PhoneNumber = customer.PhoneNumber;

            await _dbContext.SaveChangesAsync();
            return existingCustomer;
        }
    }
}

