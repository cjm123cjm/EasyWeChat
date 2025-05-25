﻿using EasyWeChat.IService.Dtos;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace EasyWeChat.Api.Middlewares
{
    /// <summary>
    /// 错误状态码处理
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                var statusCode = context.Response.StatusCode;
                if (ex is ArgumentException)
                {
                    statusCode = 200;
                }

                await HandleExceptionAsync(context, statusCode, ex.Message);
            }
            finally
            {
                var statusCode = context.Response.StatusCode;
                var msg = "";
                if (statusCode == 401)
                {
                    msg = "未授权";
                }
                else if (statusCode == 404)
                {
                    msg = "未找到服务";
                }
                else if (statusCode == 502)
                {
                    msg = "请求错误";
                }
                else if (statusCode == 403)
                {
                    msg = "禁止访问";
                }
                else if (statusCode != 200 && statusCode != 204)
                {
                    msg = "未知错误";
                }

                if (!string.IsNullOrWhiteSpace(msg))
                {
                    await HandleExceptionAsync(context, statusCode, msg);
                }
            }
        }

        //异常错误信息捕获，将错误信息用Json方式返回
        private static Task HandleExceptionAsync(HttpContext context, int statusCode, string msg)
        {
            var result = JsonConvert.SerializeObject(
                new ResponseDto
                { Code = statusCode, Message = msg },
            new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            context.Response.ContentType = "application/json;charset=utf-8";
            return context.Response.WriteAsync(result);
        }
    }
    //扩展方法　
    public static class ErrorHandlingExtensions
    {
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }

}
