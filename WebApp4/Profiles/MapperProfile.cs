using AutoMapper;
using Microsoft.AspNetCore.Identity;
using WebApp4.DTOs.School_DTOs;
using WebApp4.DTOs.Student_DTOs;
using WebApp4.DTOs.UserDTOs;
using WebApp4.Entities;
using WebApp4.Entities.Identities;

namespace WebApp4.Profiles;

public class MapperProfile:Profile
{
    public MapperProfile()
    {
        CreateMap<Student, StudentGetDTO>()
            .ForMember(dest=>dest.SchoolName,opt=>opt.MapFrom(src=>src.School.Name))
            .ReverseMap();
        CreateMap<School, SchoolGetDTO>().ReverseMap();
        CreateMap<AppUser, GetUserDTO>().ReverseMap();
        CreateMap<UpdateUserDTO, AppUser>().ReverseMap();
    }
}
