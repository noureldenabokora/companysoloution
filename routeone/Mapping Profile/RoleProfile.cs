using AutoMapper;
using Microsoft.AspNetCore.Identity;
using routeone.ViewModels;

namespace routeone.Mapping_Profile
{
    public class RoleProfile :Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleViewModel, IdentityRole>().ForMember(d => d.Name, o => o.MapFrom(s => s.RoleName))
                .ReverseMap();
        }
    }

}
