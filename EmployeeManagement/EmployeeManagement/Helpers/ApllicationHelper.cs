using AutoMapper;
using EmployeeManagement.Data;
using EmployeeManagement.Models;

namespace EmployeeManagement.Helpers
{
    public class ApllicationHelper : Profile
    {
        public ApllicationHelper()
        {
            CreateMap<Employee, EmployeeModel>().ReverseMap();
        }
    }
}
