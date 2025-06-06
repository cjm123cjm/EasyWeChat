using EasyWeChat.Domain.Entities;
using EasyWeChat.Domain.IRepository;
using EasyWeChat.Domain.Repository;
using EasyWeChat.IService.Consts;
using EasyWeChat.IService.Dtos;
using EasyWeChat.IService.Dtos.Outputs;
using EasyWeChat.IService.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;

namespace EasyWeChat.Service.Implement
{
    public class UserContactService : ServiceBase, IUserContactService
    {
        private readonly IUserContactRepository _userContactRepository;
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly IGroupInfoRepository _groupInfoRepository;
        private ResponseDto responseDto;
        public UserContactService(
            IUserContactRepository userContactRepository,
            IUserInfoRepository userInfoRepository,
            IGroupInfoRepository groupInfoRepository)
        {
            _userContactRepository = userContactRepository;
            responseDto = new ResponseDto();
            _userInfoRepository = userInfoRepository;
            _groupInfoRepository = groupInfoRepository;
        }

        /// <summary>
        /// 搜索联系人(用户id或群组id)0-用户 1-群组
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDto> GetSeachContactsAsync(long contactId, int type)
        {
            if (type == 0)
            {
                var user = await _userInfoRepository.FindByIdAsync(contactId);
                if (user == null)
                {
                    responseDto.Code = 400;
                    responseDto.Message = "没有找到id为" + contactId + "的用户";
                    return responseDto;
                }

                UserContactDto contactDto = new UserContactDto
                {
                    ContactId = user.UserId,
                    ContanctType = 0,
                    ContanctName = user.NickName,
                    Thumbnail = ServerUrl + "/upload/" + EasyWeChatConst.UserThumbnail + user.UserId + "." + user.PicExtension,
                    AreaName = user.AreaName
                };

                var seachFirends = await _userContactRepository.All().FirstOrDefaultAsync(t => t.UserId == LoginUserId && t.ContanctType == 0 && t.ContactId == contactId);
                if (seachFirends == null)
                {
                    contactDto.IsFriends = false;
                }
                else
                {
                    contactDto.IsFriends = seachFirends.Status == 1;
                }

                responseDto.Result = contactDto;

                return responseDto;
            }
            else
            {
                var group = await _groupInfoRepository.FindByIdAsync(contactId);
                if (group == null)
                {
                    responseDto.Code = 400;
                    responseDto.Message = "没有找到id为" + contactId + "的群组";
                    return responseDto;
                }

                UserContactDto contactDto = new UserContactDto
                {
                    ContactId = group.GroupId,
                    ContanctType = 0,
                    ContanctName = group.GroupName,
                    Thumbnail = ServerUrl + "/upload/" + EasyWeChatConst.GroupThumbnail + group.GroupId + "." + group.PicExtension
                };

                var seachFirends = await _userContactRepository.All().FirstOrDefaultAsync(t => t.UserId == LoginUserId && t.ContanctType == 1 && t.ContactId == contactId);
                if (seachFirends == null)
                {
                    contactDto.IsFriends = false;
                }
                else
                {
                    contactDto.IsFriends = seachFirends.Status == 1;
                }

                responseDto.Result = contactDto;

                return responseDto;
            }

        }
    }
}
