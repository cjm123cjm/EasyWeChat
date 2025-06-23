using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels.Sockets;
using DotNetty.Transport.Channels;
using System.Net;
using DotNetty.Codecs.Http;
using DotNetty.Handlers.Timeout;
using DotNetty.Codecs.Http.WebSockets;

namespace EasyWeChat.Api.WebSocket
{
    /// <summary>
    /// dotnetty服务
    /// </summary>
    public class NettyWebScoketServer
    {
        private readonly IEventLoopGroup bossGroup = new MultithreadEventLoopGroup(1);
        private readonly IEventLoopGroup workGroup = new MultithreadEventLoopGroup();
        private IChannel bootstrapChannel;
        private readonly ILogger<NettyWebScoketServer> _logger;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceProvider"></param>
        public NettyWebScoketServer(
            ILogger<NettyWebScoketServer> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 开始
        /// </summary>
        /// <returns></returns>
        public async Task RunStartAsync()
        {
            try
            {
                var bootstrap = new ServerBootstrap();
                bootstrap.Group(bossGroup, workGroup);

                bootstrap.Channel<TcpServerSocketChannel>();

                bootstrap
                       .ChildOption(ChannelOption.SoKeepalive, true)
                       .Handler(new LoggingHandler("SRV-LSTN"))
                       .Option(ChannelOption.SoBacklog, 8192)
                       .ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
                       {
                           IChannelPipeline pipeline = channel.Pipeline;
                           //对http协议的支持，使用http的编码器和解码器
                           pipeline.AddLast(new HttpServerCodec());

                           //保证接收http的完整性
                           pipeline.AddLast(new HttpObjectAggregator(65536));

                           //读超时时间，写超时时间，所有类型的超时时间
                           pipeline.AddLast(new IdleStateHandler(6, 0, 0));
                           pipeline.AddLast(new HeartBeatHandler());

                           pipeline.AddLast(new WebSocketServerProtocolHandler("/websocket", null, true, 65536, true, true));
                           pipeline.AddLast(_serviceProvider.GetRequiredService<WebSocketHandler>());
                       }));
                int port = 2222;
                bootstrapChannel = await bootstrap.BindAsync(IPAddress.Loopback, port);

                Console.WriteLine("Listening on "
                    + $"ws://127.0.0.1:{port}/websocket");
            }
            catch (Exception ex)
            {
                await ShutdownAsync();
                _logger.LogError(ex, ex.Message);
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <returns></returns>
        public async Task ShutdownAsync()
        {
            try
            {
                if (bootstrapChannel != null)
                {
                    await bootstrapChannel.CloseAsync();
                }
            }
            finally
            {
                await Task.WhenAll(
                    bossGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)),
                    workGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1))
                );
            }
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            ShutdownAsync().GetAwaiter().GetResult();
        }
    }

}
