using EasyWeChat.Common.RedisUtil;
using EasyWeChat.Domain;
using EasyWeChat.IService.Dtos;
using EasyWeChat.IService.Dtos.Outputs;
using EasyWeChat.IService.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWeChat.Service.Implement
{
    public class ChatSessionService : ServiceBase, IChatSessionService
    {
        private ResponseDto responseDto;
        private readonly EasyWeChatDbContext _context;

        public ChatSessionService(EasyWeChatDbContext context)
        {
            _context = context;
            responseDto = new ResponseDto();
        }

        /// <summary>
        /// 获取用户初始化消息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<WebSocketInitDataDto> GetUserWebSockerInitDataAsync(long userId)
        {
            WebSocketInitDataDto initData = new WebSocketInitDataDto();

            var chatSessionUserDto = await (from a in _context.ChatSessionUsers.AsNoTracking()
                                            join b in _context.ChatSessions.AsNoTracking() on a.SessionId equals b.SessionId
                                            select new ChatSessionUserDto
                                            {
                                                UserId = a.UserId,
                                                ContactId = a.ContactId,
                                                ContactType = a.ContactType,
                                                SessionId = b.SessionId,
                                                ContactName = a.ContactName,
                                                LastMessage = b.LastMessage,
                                                LastReceviceTime = b.LastReceviceTime,
                                            }).OrderByDescending(t => t.LastReceviceTime).ToListAsync();

            var groupIds = chatSessionUserDto.Where(t => t.ContactType == 1).Select(t => t.ContactId).ToList();

            var userContacts = await _context.UserContacts.AsNoTracking()
                                                         .Where(t => groupIds.Contains(t.ContactId))
                                                         .ToListAsync();
            foreach (var item in chatSessionUserDto)
            {
                if (item.ContactType == 1)
                    item.MemberCount = userContacts.Where(t => t.ContactId == item.ContactId).Count();
            }

            initData.ChatSessionUserDtos = chatSessionUserDto;

            var getUserGroupIds = CacheManager.Get<List<long>>(RedisKeyPrefix.User_Contact_Ids + userId);

            var chatMessage = await _context.ChatMessages.Where(t => t.ContactType == 0 ? t.ContactId == userId : getUserGroupIds.Contains(t.ContactId)).ToListAsync();

            initData.ChatSessionDtos = ObjectMapper.Map<List<ChatSessionDto>>(chatMessage);

            initData.ApplyCount = await _context.ApplyInfos.Where(t => t.ReceiveUserId == userId && t.Status == 0).CountAsync();

            return initData;
        }
    }
}
