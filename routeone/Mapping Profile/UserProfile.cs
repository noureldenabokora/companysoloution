using AutoMapper;
using Demo.DAL.Models;
using routeone.ViewModels;

namespace routeone.Mapping_Profile
{
    public class UserProfile : Profile
    {
        public UserProfile() 
        {
            CreateMap<AppliactionUser,UserViewModel>().ReverseMap();
        }
    }
}
