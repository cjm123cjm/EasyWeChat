using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Channels;

namespace EasyWeChat.Api.WebSocket
{
    /// <summary>
    /// 心跳检测
    /// </summary>
    public class HeartBeatHandler : ChannelDuplexHandler
    {
        public override void UserEventTriggered(IChannelHandlerContext context, object evt)
        {
            if (evt is IdleStateEvent)
            {
                IdleStateEvent idleStateEvent = (IdleStateEvent)evt;
                if (idleStateEvent.State == IdleState.ReaderIdle)
                {
                    context.WriteAndFlushAsync("心跳超时");
                    context.CloseAsync();
                }
                else if (idleStateEvent.State == IdleState.WriterIdle)
                {
                    context.WriteAndFlushAsync("heart");
                }
            }
        }
    }
}
