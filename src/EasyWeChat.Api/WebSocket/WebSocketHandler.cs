using DotNetty.Codecs.Http.WebSockets;
using DotNetty.Common.Utilities;
using DotNetty.Transport.Channels;
using EasyWeChat.Common.RedisUtil;
using EasyWeChat.IService.Dtos.Outputs;
using Newtonsoft.Json.Linq;

namespace EasyWeChat.Api.WebSocket
{
    /// <summary>
    /// WebSocketHandler
    /// </summary>
    public class WebSocketHandler : SimpleChannelInboundHandler<TextWebSocketFrame>
    {
        private readonly ILogger<WebSocketHandler> _logger;
        private readonly ChannelContextUtils _channelContextUtils;

        public WebSocketHandler(
            ILogger<WebSocketHandler> logger, ChannelContextUtils channelContextUtils)
        {
            _logger = logger;
            _channelContextUtils = channelContextUtils;
        }
        protected override void ChannelRead0(IChannelHandlerContext ctx, TextWebSocketFrame msg)
        {
            var channel = ctx.Channel;

            var attributeKey = channel.GetAttribute(AttributeKey<string>.ValueOf(channel.Id.ToString())).Get();

            _logger.LogInformation("收到" + attributeKey + "的消息：" + msg.Text());

            //添加到redis 表示上线了
            CacheManager.Set(RedisKeyPrefix.Heart + attributeKey, TimeSpan.FromSeconds(6));
        }
        public override void ChannelActive(IChannelHandlerContext context)
        {
            Console.WriteLine("有新的链接加入...");
        }
        public override void ChannelInactive(IChannelHandlerContext context)
        {
            Console.WriteLine("有链接断开...");

            _channelContextUtils.RemoveContext(context.Channel);
        }
        public override void UserEventTriggered(IChannelHandlerContext context, object evt)
        {
            if (evt is WebSocketServerProtocolHandler.HandshakeComplete)
            {
                WebSocketServerProtocolHandler.HandshakeComplete complete = (WebSocketServerProtocolHandler.HandshakeComplete)evt;
                string url = complete.RequestUri.ToString();
                string? token = getToken(url);
                if (token == null)
                {
                    context.Channel.CloseAsync();
                    return;
                }
                UserInfoDto userInfo = CacheManager.Get<UserInfoDto>(RedisKeyPrefix.Online + token);
                if (userInfo == null)
                {
                    context.Channel.CloseAsync();
                    return;
                }

                _channelContextUtils.AddContext(context.Channel, userInfo.UserId.ToString());
            }
        }
        private string? getToken(string url)
        {
            if (string.IsNullOrEmpty(url) || url.IndexOf("?") == -1)
            {
                return null;
            }
            string[] split = url.Split('?');
            if (split.Length != 2)
            {
                return null;
            }
            string[] parambers = split[1].Split("=");
            if (parambers.Length != 2)
            {
                return null;
            }
            return parambers[1];
        }
    }

}
