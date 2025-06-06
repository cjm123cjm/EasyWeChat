using EasyWeChat.Domain.IRepository;
using EasyWeChat.Domain.Repository;
using EasyWeChat.IService.Consts;
using EasyWeChat.IService.Dtos;
using EasyWeChat.IService.Dtos.Inputs;
using EasyWeChat.IService.Dtos.Outputs;
using EasyWeChat.IService.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EasyWeChat.Service.Implement
{
    public class AdminService : ServiceBase, IAdminService
    {
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly IGroupInfoRepository _groupInfoRepository;
        private ResponseDto responseDto;

        public AdminService(
            IUserInfoRepository userInfoRepository,
            IGroupInfoRepository groupInfoRepository)
        {
            _userInfoRepository = userInfoRepository;
            responseDto = new ResponseDto();
            _groupInfoRepository = groupInfoRepository;
        }

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDto> GetUserInfosAsync(UserQueryInput userQueryInput)
        {
            var query = _userInfoRepository.All();
            if (userQueryInput.UserId.HasValue)
            {
                query = query.Where(t => t.UserId == userQueryInput.UserId.Value);
            }
            if (!string.IsNullOrWhiteSpace(userQueryInput.NickName))
            {
                query = query.Where(t => t.NickName != null && t.NickName.Contains(userQueryInput.NickName));
            }

            var users = await query.ToListAsync();

            var dtos = ObjectMapper.Map<List<UserInfoDto>>(users);

            dtos.ForEach(t =>
            {
                t.Thumbnail = ServerUrl + "/upload/" + EasyWeChatConst.UserThumbnail + t.UserId + "." + t.PicExtension;
            });

            responseDto.Result = dtos;

            return responseDto;
        }

        /// <summary>
        /// 修改用户状态，状态 0：启用 1：禁用
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status">状态 0：启用 1：禁用</param>
        /// <returns></returns>
        public async Task<ResponseDto> UpdateUserStateAsync(long userId, int status)
        {
            if (status != 0 && status != 1)
            {
                responseDto.Message = "参数错误";
                responseDto.Code = 400;
                return responseDto;
            }

            var user = await _userInfoRepository.FindByIdAsync(userId);
            if (user == null)
            {
                responseDto.Message = "用户不存在";
                responseDto.Code = 400;
                return responseDto;
            }

            user.Status = status;

            return responseDto;
        }

        /// <summary>
        /// 强制下线
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ResponseDto> ForceOfflineAsync(long userId)
        {
            return responseDto;
        }

        /// <summary>
        /// 获取所有群组
        /// </summary>
        /// <param name="groupInfoQueryInput"></param>
        /// <returns></returns>
        public async Task<ResponseDto> GetGroupInfosAsync(GroupInfoQueryInput groupInfoQueryInput)
        {
            var query = _groupInfoRepository.All();
            if (groupInfoQueryInput.GroupId.HasValue)
            {
                query = query.Where(t => t.GroupId == groupInfoQueryInput.GroupId.Value);
            }
            if (groupInfoQueryInput.GroupOwnerId.HasValue)
            {
                query = query.Where(t => t.GroupOwnerId == groupInfoQueryInput.GroupOwnerId.Value);
            }
            if (!string.IsNullOrWhiteSpace(groupInfoQueryInput.GroupName))
            {
                query = query.Where(t => t.GroupName.Contains(groupInfoQueryInput.GroupName));
            }

            var groupInfos = await query.ToListAsync();

            var groupInfoDto = ObjectMapper.Map<List<GroupInfoDto>>(groupInfos);

            var userIds = groupInfoDto.Select(t => t.GroupOwnerId).Distinct().ToList();
            var userInfos = await _userInfoRepository.All().Where(t => userIds.Contains(t.UserId)).ToListAsync();

            groupInfoDto.ForEach(t =>
            {
                t.GroupOwnerName = userInfos.FirstOrDefault(m => m.UserId == t.GroupOwnerId)?.NickName;
                t.Thumbnail = ServerUrl + "/upload/" + EasyWeChatConst.GroupThumbnail + t.GroupId + "." + t.PicExtension;
            });

            responseDto.Result = groupInfoDto;

            return responseDto;
        }
    }
}
