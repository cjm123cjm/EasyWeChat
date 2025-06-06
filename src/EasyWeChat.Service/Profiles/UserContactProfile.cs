using AutoMapper;
using EasyWeChat.Domain.Entities;
using EasyWeChat.IService.Dtos.Outputs;

namespace EasyWeChat.Service.Profiles
{
    public class UserContactProfile : Profile
    {
        public UserContactProfile()
        {
            CreateMap<UserContactDto, UserContact>().ReverseMap();
        }
    }
}
