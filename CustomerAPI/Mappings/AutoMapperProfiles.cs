using AutoMapper;
using CustomerAPI.Models;
using CustomerAPI.Models.DTO;

namespace CustomerAPI.Mappings
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<AddCustomerRequestDto, Customer>().ReverseMap();
            CreateMap<UpdateCustomerRequestDto, Customer>().ReverseMap();
        }
    }
}
