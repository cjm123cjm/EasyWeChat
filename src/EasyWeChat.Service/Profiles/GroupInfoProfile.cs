using AutoMapper;
using EasyWeChat.Domain.Entities;
using EasyWeChat.IService.Dtos.Inputs;
using EasyWeChat.IService.Dtos.Outputs;

namespace EasyWeChat.Service.Profiles
{
    public class GroupInfoProfile : Profile
    {
        public GroupInfoProfile()
        {
            CreateMap<GroupInfoInput, GroupInfo>().ReverseMap();

            CreateMap<GroupInfoDto, GroupInfo>().ReverseMap();
        }
    }
}
