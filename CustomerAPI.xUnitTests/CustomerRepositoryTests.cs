using AutoMapper;
using CustomerAPI.Controllers;
using CustomerAPI.Data;
using CustomerAPI.Models;
using CustomerAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerAPI.xUnitTests
{
    public class CustomerRepositoryTests
    {
        private readonly CustomerDbContext _context;
        private readonly SQLCustomerRepository customerRepository;
        private readonly CustomersController customersController;
        private readonly IMapper _mapper;
        public CustomerRepositoryTests()
        {
             DbContextOptions<CustomerDbContext> dbContextOptions = new DbContextOptionsBuilder<CustomerDbContext>()
            .UseInMemoryDatabase(databaseName: "CustomerDBTest" + Guid.NewGuid().ToString())
            .Options;

        _context = new CustomerDbContext(dbContextOptions);
            _context.Database.EnsureCreated();
            SeedDatabase();

            customerRepository = new SQLCustomerRepository(_context);
            customersController = new CustomersController(_context, customerRepository, _mapper);

        }

        [Fact]
        public void GetCustomerById_ReturnsCorrectCustomer()
        {
            var customerId = Guid.Parse("4f681475-39dd-47c6-a91d-ff1f7e95430f");
            var customerFirstName = "Robert";

            var result = customerRepository.GetCustomerByIdAsync(customerId);

            Assert.NotNull(result);
            Assert.Equal(customerId.ToString(), result.Result.Id.ToString());
            Assert.Equal(customerFirstName, result.Result.FirstName);
        }

        [Fact]
        public void GetCustomerById_ReturnsNullWhenCustomerNotFound()
        {
            var customerId = Guid.Parse("a12903ca-8b7e-4eeb-9630-7202952a8511");

            var result = customerRepository.GetCustomerByIdAsync(customerId);

            Assert.Null(result.Result);
        }

        [Fact]
        public void GetAllCustomers_ReturnsAllCustomers()
        {
            var result = customerRepository.GetAllCustomersAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Result.Count());
        }

        [Fact]
        public void AddCustomer_AddCustomerCorrectly()
        {
            var newCustomer = new Customer { Id = Guid.Parse("c7e50ee2-ef97-4d11-925d-4ee078f0f5b3"), FirstName = "Jose", LastName = "Ramirez", PhoneNumber = "3234" };

            customerRepository.AddCustomerAsync(newCustomer);
            var result = customerRepository.GetCustomerByIdAsync(Guid.Parse("c7e50ee2-ef97-4d11-925d-4ee078f0f5b3"));

            Assert.NotNull(newCustomer);
            Assert.Equal(newCustomer.Id, result.Result.Id);
            Assert.Equal(newCustomer.FirstName, result.Result.FirstName);
            Assert.Equal(newCustomer.LastName, result.Result.LastName);
            Assert.Equal(newCustomer.PhoneNumber, result.Result.PhoneNumber);

        }

        [Fact]
        public void UpdateCustomer_UpdateCustomersCorrectly()
        {
            var updatedCustomer = new Customer { Id = Guid.Parse("373aade2-62a2-4e4f-8046-1d96e4bb41f9"), FirstName = "Updated Customer", LastName = "Updated Lastname", PhoneNumber = "214-093-9870" };

            customerRepository.UpdateCustomerAsync(updatedCustomer.Id, updatedCustomer);
            var result = customerRepository.GetCustomerByIdAsync(Guid.Parse("373aade2-62a2-4e4f-8046-1d96e4bb41f9"));

            Assert.NotNull(result);
            Assert.Equal(updatedCustomer.FirstName, result.Result.FirstName);
            Assert.Equal(updatedCustomer.LastName, result.Result.LastName);
            Assert.Equal(updatedCustomer.PhoneNumber, result.Result.PhoneNumber);
        }

        [Fact]
        public void DeleteCustomer_DeleteCustomersCorrectly()
        {
            var customerId = Guid.Parse("373aade2-62a2-4e4f-8046-1d96e4bb41f9");

            customerRepository.DeleteCustomerAsync(customerId);
            var result = customerRepository.GetCustomerByIdAsync(customerId);

            Assert.Null(result.Result);
        }
        private void SeedDatabase()
        {
            var customer = new List<Customer>()
            {
                new Customer()
                {
                    Id = Guid.Parse("4f681475-39dd-47c6-a91d-ff1f7e95430f"),
                    FirstName = "Robert",
                    LastName = "De Niro",
                    PhoneNumber = "1234567890",
                },
                new Customer()
                {
                    Id = Guid.Parse("373aade2-62a2-4e4f-8046-1d96e4bb41f9"),
                    FirstName = "Charles",
                    LastName = "Darwin",
                    PhoneNumber = "1234567891",
                }
            }; 
            _context.Customers.AddRange(customer);

            _context.SaveChanges();

        }
    }
}
