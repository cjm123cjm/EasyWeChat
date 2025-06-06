using AutoMapper;
using EasyWeChat.IService.Dtos;
using EasyWeChat.IService.Dtos.Inputs;

namespace EasyWeChat.Service.Profiles
{
    public class AdminProfile : Profile
    {
        public AdminProfile()
        {
            CreateMap<SystemSettingDto, SystemSettingInput>().ReverseMap();
        }
    }
}
