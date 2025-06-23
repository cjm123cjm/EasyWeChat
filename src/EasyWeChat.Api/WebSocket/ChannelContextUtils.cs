using DotNetty.Codecs.Http.WebSockets;
using DotNetty.Common.Concurrency;
using DotNetty.Common.Utilities;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Groups;
using EasyWeChat.Common.RedisUtil;
using EasyWeChat.IService.Dtos.Outputs;
using EasyWeChat.IService.Enums;
using EasyWeChat.IService.Interfaces;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace EasyWeChat.Api.WebSocket
{
    public class ChannelContextUtils
    {
        private static ConcurrentDictionary<string, IChannel> userDictionary = new ConcurrentDictionary<string, IChannel>();
        private static ConcurrentDictionary<string, IChannelGroup> groupDictionary = new ConcurrentDictionary<string, IChannelGroup>();
        private readonly IUserService _userService;
        private readonly IChatSessionService _chatSessionService;
        private readonly IEventExecutorGroup _executorGroup;

        public ChannelContextUtils(IUserService userService, IChatSessionService chatSessionService)
        {
            _userService = userService;
            // 初始化线程池（推荐共享使用）
            _executorGroup = new MultithreadEventLoopGroup(Environment.ProcessorCount);
            _chatSessionService = chatSessionService;
        }

        public async void AddContext(IChannel channel, string userId)
        {
            string channelId = channel.Id.ToString()!;

            channel.GetAttribute(AttributeKey<string>.ValueOf(channelId)).Set(userId);

            userDictionary.TryAdd(userId, channel);

            CacheManager.Set(RedisKeyPrefix.Heart + userId, TimeSpan.FromSeconds(6));

            var contactIds = CacheManager.Get<List<long>>(RedisKeyPrefix.User_Contact_Ids + userId);
            foreach (var contactId in contactIds)
            {
                AddGroupContext(contactId.ToString(), channel);
            }

            //更新用户最后连接时间
            await _userService.UpdateLastLoginTimeAsync(Convert.ToInt64(userId));

            //查询用户：1-会话消息，2-查询聊天记录 3-查询好友申请
            var wsInitData = await _chatSessionService.GetUserWebSockerInitDataAsync(Convert.ToInt64(userId));

            MessageSendDto<WebSocketInitDataDto> messageSendDto = new MessageSendDto<WebSocketInitDataDto>();
            messageSendDto.MessageType = MessageTypeEnum.INIT;
            messageSendDto.ContactId = Convert.ToInt64(userId);
            messageSendDto.ExtendData = wsInitData;

            SendMessage(messageSendDto, userId);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="messageSendDto"></param>
        /// <param name="reciveId">接收人id</param>
        public static void SendMessage(MessageSendDto<WebSocketInitDataDto> messageSendDto, string reciveId)
        {
            if (reciveId == null) return;

            IChannel channel = userDictionary[reciveId];
            if (channel == null) return;

            messageSendDto.ContactId = messageSendDto.SendUserId;
            messageSendDto.ContactName = messageSendDto.SendUserNikcName;

            channel.WriteAndFlushAsync(new TextWebSocketFrame(JsonConvert.SerializeObject(messageSendDto)));
        }

        private void AddGroupContext(string groupId, IChannel channel)
        {
            IChannelGroup groupChannel = groupDictionary[groupId];
            if (groupChannel == null)
            {
                //IEventExecutor
                groupChannel = new DefaultChannelGroup(_executorGroup.GetNext());
                groupDictionary.TryAdd(groupId, groupChannel);
            }
            if (channel == null)
                return;

            groupChannel.Add(channel);
        }

        /// <summary>
        /// 断开连接,移除channel
        /// </summary>
        /// <param name="channel"></param>
        public async void RemoveContext(IChannel channel)
        {
            var userId = channel.GetAttribute(AttributeKey<string>.ValueOf(channel.Id.ToString())).Get();
            if (!string.IsNullOrEmpty(userId))
            {
                userDictionary.Remove(userId, out _);
            }

            CacheManager.Remove(RedisKeyPrefix.Heart + userId);

            //更新用户最后更新时间
            await _userService.UpdateLastOffTimeAsync(Convert.ToInt64(userId));
        }

        //public void SendGroupMessage(string message)
        //{
        //    IChannelGroup groupChannel = groupDictionary["1001"];
        //    groupChannel.WriteAndFlushAsync(message);
        //}
    }
}
