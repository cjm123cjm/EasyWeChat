using AutoMapper;
using EasyWeChat.Common.Snowflake;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace EasyWeChat.Service
{
    public class ServiceBase
    {
        protected long LoginUserId { get; set; }
        protected string LoginUserName { get; set; }
        protected long LoginRoleId { get; set; }
        protected IMapper ObjectMapper { get; set; }
        protected string ServerUrl { get; set; }
        protected IdWorker SnowIdWorker { get; set; }
        public ServiceBase()
        {
            var httpContext = LocationStorage.Instance.GetService<IHttpContextAccessor>()!;
            if (httpContext.HttpContext != null && httpContext.HttpContext.User != null && httpContext.HttpContext.User.Identity != null)
            {
                if (httpContext.HttpContext!.User!.Identity!.IsAuthenticated)
                {
                    LoginUserId = Convert.ToInt64(httpContext.HttpContext.User.Claims.First(t => t.Type == "UserId").Value);
                    LoginRoleId = Convert.ToInt64(httpContext.HttpContext.User.Claims.First(t => t.Type == "RoleId").Value);
                    LoginUserName = httpContext.HttpContext.User.Claims.First(t => t.Type == "UserName").Value.ToString();
                    ServerUrl = $"{httpContext.HttpContext.Request.Scheme}://{httpContext.HttpContext.Request.Host}";
                }
            }

            ObjectMapper = LocationStorage.Instance.GetService<IMapper>()!;

            SnowIdWorker = SnowflakeUtil.CreateIdWorker();
        }
    }

}
