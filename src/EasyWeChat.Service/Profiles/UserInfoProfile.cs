using AutoMapper;
using EasyWeChat.Domain.Entities;
using EasyWeChat.IService.Dtos.Inputs;
using EasyWeChat.IService.Dtos.Outputs;

namespace EasyWeChat.Service.Profiles
{
    public class UserInfoProfile : Profile
    {
        public UserInfoProfile()
        {
            CreateMap<UserInfoDto, UserInfo>().ReverseMap();

            CreateMap<UserInfoInput, UserInfo>().ReverseMap();
        }
    }
}
