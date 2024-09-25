using AutoFixture;
using AutoMapper;
using Castle.Core.Resource;
using CustomerAPI.Controllers;
using CustomerAPI.Data;
using CustomerAPI.Models;
using CustomerAPI.Models.DTO;
using CustomerAPI.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using static NuGet.Packaging.PackagingConstants;

namespace CustomerAPI.xUnitTests
{
    public class CustomersControllerTests
    {
        private readonly CustomersController _controller;

        private readonly Mock<ICustomerRepository> _mockRepository = new Mock<ICustomerRepository>();
        private readonly CustomerDbContext _context;
        private IMapper _mapper;
        private Fixture fixture;

        public CustomersControllerTests()
        {
            DbContextOptions<CustomerDbContext> dbContextOptions = new DbContextOptionsBuilder<CustomerDbContext>()
           .UseInMemoryDatabase(databaseName: "CustomerDBTest" + Guid.NewGuid().ToString())
           .Options;

            _context = new CustomerDbContext(dbContextOptions);
            _context.Database.EnsureCreated();
            SeedDatabase();

            fixture = new Fixture();
            _controller = new CustomersController(_context, _mockRepository.Object, _mapper);
        }

        [Fact]
        public async Task GetCustomerById_ReturnsCustomer_WhenCustomerExists()
        {
            var custId = Guid.Parse("A5D46C1A-B9E3-4E36-770D-08DCDCB6882F");

            var custRepo = new Customer
            {
                Id = custId,
                FirstName = "Leonardo",
                LastName = "Di Carpio",
                PhoneNumber = "1234567890"
            };

            _mockRepository.Setup(x => x.GetCustomerByIdAsync(custId))
                .ReturnsAsync(custRepo);

            var customer = _controller.GetCustomersById(custId) ;
            var custResult = customer.Result as OkObjectResult;
            var custValue = custResult.Value as Customer;

            Assert.NotNull(custValue);
            Assert.Equal(custId.ToString(), custValue.Id.ToString());
            Assert.Equal(custRepo.FirstName, custValue.FirstName);
            Assert.Equal(custRepo.LastName, custValue.LastName);
            Assert.Equal(custRepo.PhoneNumber, custValue.PhoneNumber);

        }

        [Fact]
        public async Task GetCustomerById_ReturnsNullWhenCustomerNotFound()
        {
            var custId = Guid.Parse("A5D46C1A-B9E3-4E36-770D-08DCDCB6882D");
            _mockRepository.Setup(x => x.GetCustomerByIdAsync(custId))
                .ReturnsAsync((Customer)null);
            
            var result = _controller.GetCustomersById(custId);
            var custResult = result.Result as NotFoundObjectResult;

            Assert.Null(custResult);
        }

        [Fact]
        public async Task GetAllCustomers_ReturnsAllCustomers()
        {
            var customers = fixture.CreateMany<Customer>(3).ToList();

            _mockRepository.Setup(x => x.GetAllCustomersAsync()).ReturnsAsync(customers);

            var result = _controller.GetAllCustomers();

            var obj = result.Result as OkObjectResult;

            Assert.Equal(200, obj.StatusCode);

         }
        private List<Customer> GetCustomersData()
        {
            List<Customer> custData = new List<Customer>
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
                },
            };

            return custData;
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
