using Autofac.Extensions.DependencyInjection;
using Autofac;
using EasyWeChat.Api.Extensions;
using Serilog;
using Microsoft.AspNetCore.Mvc;
using EasyWeChat.IService.Dtos;
using EasyWeChat.Domain;
using Microsoft.EntityFrameworkCore;
using IGeekFan.AspNetCore.Knife4jUI;
using EasyWeChat.Api.Middlewares;
using EasyWeChat.Service;
using EasyWeChat.Api.WebSocket;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

//swagger添加jwt认证
builder.AddSwaggerAuth();

//autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule<AutofacModuleRegister>();
    });

//jwt认证
builder.AddAuthetication();

// 配置允许跨域
builder.AddCustomerCors();

//配置日志serilog
builder.Host.UseSerilog((context, logger) =>
{
    //Serilog读取配置
    logger.ReadFrom.Configuration(context.Configuration);
    logger.Enrich.FromLogContext();
});

//AutoMapper
builder.Services.AddAutoMapper(p =>
{
    p.AddMaps("EasyWeChat.Service");
});

builder.Services.AddHttpContextAccessor();

//添加模型验证
builder.Services.AddOptions().Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errorInfo = new ValidationProblemDetails(context.ModelState).Errors
             .Select(t => $"{t.Key}:{string.Join(",", t.Value)}");
        return new OkObjectResult(new ResponseDto
        {
            Code = 400,
            Message = string.Join("\r\n", errorInfo)
        });
    };
});

//注入dbcontext
builder.Services.AddDbContext<EasyWeChatDbContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("mysql"), new MySqlServerVersion("5.7"));
});

//添加权限策略
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("IsAdmin", policy =>
    {
        policy.RequireClaim("IsAdmin", "true");
    });
});

//注册服务
builder.Services.AddSingleton<NettyWebScoketServer>();
builder.Services.AddTransient<WebSocketHandler>();
builder.Services.AddTransient<ChannelContextUtils>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    //app.UseSwaggerUI();
    // 自定义的UI
    app.UseKnife4UI();
}

//启动工作流
var host = app.Services.GetRequiredService<NettyWebScoketServer>();
var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
lifetime.ApplicationStarted.Register(() =>
{
    _ = host.RunStartAsync();
});
lifetime.ApplicationStopping.Register(() =>
{
    _ = host.ShutdownAsync();
});

//错误中间件
app.UseErrorHandling();

LocationStorage.Instance = app.Services;

app.UseCors("EasyWeChat.Client");

app.UseStaticFiles();

//认证
app.UseAuthentication();
//授权
app.UseAuthorization();

app.MapControllers();

app.Run();
