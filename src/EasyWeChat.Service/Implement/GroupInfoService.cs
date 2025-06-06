using EasyWeChat.Common.RedisUtil;
using EasyWeChat.Domain.Entities;
using EasyWeChat.Domain.IRepository;
using EasyWeChat.IService.Consts;
using EasyWeChat.IService.Dtos;
using EasyWeChat.IService.Dtos.Inputs;
using EasyWeChat.IService.Dtos.Outputs;
using EasyWeChat.IService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EasyWeChat.Service.Implement
{
    public class GroupInfoService : ServiceBase, IGroupInfoService
    {
        private readonly IGroupInfoRepository _groupInfoRepository;
        private readonly IUserContactRepository _userContactRepository;

        private ResponseDto responseDto;

        public GroupInfoService(
            IGroupInfoRepository groupInfoRepository,
            IUserContactRepository userContactRepository)
        {
            _groupInfoRepository = groupInfoRepository;
            _userContactRepository = userContactRepository;
            responseDto = new ResponseDto();
        }

        /// <summary>
        /// 创建/修改群组
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDto> CreateGroupInfoAsync(GroupInfoInput groupInfoInput)
        {
            var extension = groupInfoInput.avatarFile == null ? "" : Path.GetExtension(groupInfoInput.avatarFile.FileName);

            GroupInfo? group = null;
            if (groupInfoInput.GroupId == 0)
            {
                //判断群组有没有大于系统设置的最大群组数
                var system = CacheManager.Get<SystemSettingDto>(RedisKeyPrefix.SystemSeting);
                var count = await _groupInfoRepository.All().Where(t => t.Status == 1 && t.GroupOwnerId == LoginUserId).CountAsync();
                if (count > system.MaxGroupCount)
                {
                    responseDto.Code = 400;
                    responseDto.Message = "群组数量已经大于" + system.MaxGroupCount + "个,无法创建群组";
                    return responseDto;
                }

                group = ObjectMapper.Map<GroupInfo>(groupInfoInput);

                group.GroupId = SnowIdWorker.NextId();
                group.GroupOwnerId = LoginUserId;
                group.PicExtension = extension;

                await _groupInfoRepository.AddAsync(group);

                //将群组添加为联系人
                UserContact userContact = new UserContact
                {
                    UserId = LoginUserId,
                    ContactId = group.GroupId,
                    ContanctType = 1,
                    Status = 1,
                    LastUpdatedTime = DateTime.Now,
                };
                await _userContactRepository.AddAsync(userContact);

                //todo:发送消息
            }
            else
            {
                group = await _groupInfoRepository.FindByIdAsync(groupInfoInput.GroupId);
                if (group == null)
                {
                    responseDto.Code = 400;
                    responseDto.Message = "群组不存在";
                    return responseDto;
                }

                if (group.GroupOwnerId != LoginUserId)
                {
                    responseDto.Code = 400;
                    responseDto.Message = "您不是该群组的群主,无法修改群组信息";
                    return responseDto;
                }

                ObjectMapper.Map(group, groupInfoInput);

                group.PicExtension = extension;

                //todo：发送消息

                await _groupInfoRepository.UpdateAsync(group);
            }

            responseDto.Result = group.GroupId;



            return responseDto;
        }

        /// <summary>
        /// 解散群聊
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public async Task<ResponseDto> DissolutionGroupAsync(long groupId)
        {
            var group = await _groupInfoRepository.FindByIdAsync(groupId);
            if (group == null)
            {
                responseDto.Code = 400;
                responseDto.Message = "群聊不存在";
                return responseDto;
            }

            if (IsAdmin)
            {
                group.Status = 0;
            }
            else
            {
                if (group.GroupOwnerId != LoginUserId)
                {
                    responseDto.Code = 400;
                    responseDto.Message = "不是群主无法解散群聊";
                    return responseDto;
                }
                else
                {
                    group.Status = 0;
                }
            }

            await _groupInfoRepository.UpdateAsync(group);

            var userContacts = await _userContactRepository.GetAllWhere(t => t.ContanctType == 1 && t.ContactId == group.GroupId).ToListAsync();
            userContacts.ForEach(t =>
            {
                t.Status = 3;
            });

            await _userContactRepository.UpdateAsync(userContacts.ToArray());

            //TODO 移除相关的缓存信息

            //TODO 发消息 1、更新会话记录 2、记录群消息 3、发送解散通知消息

            return responseDto;
        }

        /// <summary>
        /// 获取某个群组详情
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public async Task<ResponseDto> GetGroupInfoAsync(long groupId)
        {
            var group = await _groupInfoRepository.FindByIdAsync(groupId);
            if (group == null)
            {
                responseDto.Code = 400;
                responseDto.Message = "群聊不存在";
                return responseDto;
            }

            if (group.Status == 1)
            {
                responseDto.Code = 400;
                responseDto.Message = "该群已解散";
                return responseDto;
            }

            var any = await _userContactRepository.All().AnyAsync(t => t.UserId == LoginUserId && t.ContactId == groupId && t.ContanctType == 1);
            if (!any)
            {
                responseDto.Code = 400;
                responseDto.Message = "你不在群聊";
                return responseDto;
            }

            var groupInfoDto = ObjectMapper.Map<GroupInfoDto>(group);

            //查询该群有多少人
            groupInfoDto.UserCount = await _userContactRepository.All().Where(t => t.ContactId == groupId && t.ContanctType == 1).CountAsync();

            //查询头像地址
            groupInfoDto.Thumbnail = ServerUrl + "/upload/" + EasyWeChatConst.GroupThumbnail + groupInfoDto.GroupId + "." + group.PicExtension;

            responseDto.Result = groupInfoDto;

            return responseDto;
        }

        /// <summary>
        /// 我的群组
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDto> GetMyGroupInfosAsync()
        {
            var groupInfos = await _groupInfoRepository.All().Where(t => t.GroupOwnerId == LoginUserId).ToListAsync();

            List<UserContactDto> userContactDtos = new List<UserContactDto>();

            foreach (var groupInfo in groupInfos)
            {
                userContactDtos.Add(new UserContactDto
                {
                    ContactId = groupInfo.GroupId,
                    ContanctType = 1,
                    ContanctName = groupInfo.GroupName,
                    Thumbnail = ServerUrl + "/upload/" + EasyWeChatConst.GroupThumbnail + groupInfo.GroupId + "." + groupInfo.PicExtension
                });
            }

            responseDto.Result = userContactDtos;

            return responseDto;
        }

        /// <summary>
        /// 我的加入群组
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseDto> GetMyJoinGroupInfosAsync()
        {
            var userContactDtos = await (from a in _userContactRepository.All().Where(t => t.UserId == LoginUserId)
                                         join b in _groupInfoRepository.All().Where(t => t.GroupOwnerId != LoginUserId) on a.ContactId equals b.GroupId
                                         select new UserContactDto
                                         {
                                             ContactId = b.GroupId,
                                             ContanctType = 1,
                                             ContanctName = b.GroupName,
                                             Thumbnail = ServerUrl + "/upload/" + EasyWeChatConst.GroupThumbnail + b.GroupId + "." + b.PicExtension
                                         }).ToListAsync();

            responseDto.Result = userContactDtos;

            return responseDto;
        }
    }
}
