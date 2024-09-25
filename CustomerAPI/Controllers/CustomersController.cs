using AutoMapper;
using CustomerAPI.Data;
using CustomerAPI.Models;
using CustomerAPI.Models.DTO;
using CustomerAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;

namespace CustomerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly CustomerDbContext _dbContext;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomersController(CustomerDbContext dbContext, ICustomerRepository customerRepository, IMapper mapper)
        {
            this._dbContext = dbContext;
            this._customerRepository = customerRepository;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await _customerRepository.GetAllCustomersAsync();

            //var customersDto = _mapper.Map<List<CustomerDto>>(customers);

            return Ok(customers);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetCustomersById([FromRoute] Guid id)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(id);
            if(customer == null)
            {
                return NotFound();
            }

           // var customerDto = _mapper.Map<CustomerDto>(customer);

            return Ok(customer);

        }

        [HttpPost]
        public async Task<IActionResult> AddCustomer([FromBody] AddCustomerRequestDto addCustomerRequestDto)
        {
            var customer = _mapper.Map<Customer>(addCustomerRequestDto);

            customer = await _customerRepository.AddCustomerAsync(customer);

            var customerDto = _mapper.Map<CustomerDto>(customer);

            return CreatedAtAction(nameof(GetCustomersById), new { id = customerDto.Id }, customerDto);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateCustomer([FromRoute] Guid id, [FromBody] UpdateCustomerRequestDto updateCustomerRequestDto)
        {
            var customer = _mapper.Map<Customer>(updateCustomerRequestDto);

            customer = await _customerRepository.UpdateCustomerAsync(id, customer);
            if (customer == null)
            {
                return NotFound();
            }

            var customerDto = _mapper.Map<CustomerDto>(customer);

            return Ok(customerDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteCustomer([FromRoute] Guid id)
        {
            var customer = await _customerRepository.DeleteCustomerAsync(id);
            if(customer == null)
            {
                return NotFound();
            }

            var customerDto = _mapper.Map<CustomerDto>(customer);

            return Ok(customerDto);
        }
    }
}
