using AutoMapper;
using Demo.DAL.Models;
using routeone.ViewModels;

namespace routeone.Mapping_Profile
{
    public class EmployeeProfile :Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();
        }
    }
}
