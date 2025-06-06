using AutoMapper;
using EasyWeChat.Domain.Entities;
using EasyWeChat.IService.Dtos.Inputs;
using EasyWeChat.IService.Dtos.Outputs;

namespace EasyWeChat.Service.Profiles
{
    public class ApplyInfoProfile : Profile
    {
        public ApplyInfoProfile()
        {
            CreateMap<ApplyInfoInput, ApplyInfo>().ReverseMap();

            CreateMap<ApplyInfoDto, ApplyInfo>().ReverseMap();
        }
    }
}
