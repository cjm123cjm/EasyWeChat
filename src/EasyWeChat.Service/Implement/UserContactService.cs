﻿using EasyWeChat.Domain.Entities;
using EasyWeChat.Domain.IRepository;
using EasyWeChat.Domain.Repository;
using EasyWeChat.IService.Consts;
using EasyWeChat.IService.Dtos;
using EasyWeChat.IService.Dtos.Inputs;
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
        public async Task<ResponseDto> GetSeachContactsAsync(SearchContactQueryInput queryInput)
        {
            if (queryInput.ContactType == 0)
            {
                var user = await _userInfoRepository.FindByIdAsync(queryInput.ContactId);
                if (user == null)
                {
                    responseDto.Code = 400;
                    responseDto.Message = "没有找到id为" + queryInput.ContactId + "的用户";
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

                var seachFirends = await _userContactRepository.All().FirstOrDefaultAsync(t => t.UserId == LoginUserId && t.ContanctType == 0 && t.ContactId == queryInput.ContactId);
                if (seachFirends == null)
                {
                    contactDto.Status = 0;
                }
                else
                {
                    contactDto.Status = seachFirends.Status;
                }

                responseDto.Result = contactDto;

                return responseDto;
            }
            else
            {
                var group = await _groupInfoRepository.FindByIdAsync(queryInput.ContactId);
                if (group == null)
                {
                    responseDto.Code = 400;
                    responseDto.Message = "没有找到id为" + queryInput.ContactId + "的群组";
                    return responseDto;
                }

                UserContactDto contactDto = new UserContactDto
                {
                    ContactId = group.GroupId,
                    ContanctType = 0,
                    ContanctName = group.GroupName,
                    Thumbnail = ServerUrl + "/upload/" + EasyWeChatConst.GroupThumbnail + group.GroupId + "." + group.PicExtension
                };

                var seachFirends = await _userContactRepository.All().FirstOrDefaultAsync(t => t.UserId == LoginUserId && t.ContanctType == 1 && t.ContactId == queryInput.ContactId);
                if (seachFirends == null)
                {
                    contactDto.Status = 0;
                }
                else
                {
                    contactDto.Status = seachFirends.Status;
                }

                responseDto.Result = contactDto;

                return responseDto;
            }

        }
    }
}
