using AutoMapper;
using EasyWeChat.Domain.Entities;
using EasyWeChat.IService.Dtos.Inputs;

namespace EasyWeChat.Service.Profiles
{
    public class AppUpdateProfile : Profile
    {
        public AppUpdateProfile()
        {
            CreateMap<AppUpdate, AppUpdateInput>().ReverseMap();
        }
    }
}
