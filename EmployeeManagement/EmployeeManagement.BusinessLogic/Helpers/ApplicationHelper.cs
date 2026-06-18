using AutoMapper;
using EmployeeManagement.Models;

namespace EmployeeManagement.BusinessLogic.Mapping
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeCreateRequest, EmployeeModel>().ReverseMap();

            CreateMap<EmployeeUpdateRequest, EmployeeModel>().ReverseMap();
        }
    }
}