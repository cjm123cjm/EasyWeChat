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

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

//swagger���jwt��֤
builder.AddSwaggerAuth();

//autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule<AutofacModuleRegister>();
    });

//jwt��֤
builder.AddAuthetication();

// �����������
builder.AddCustomerCors();

//������־serilog
builder.Host.UseSerilog((context, logger) =>
{
    //Serilog��ȡ����
    logger.ReadFrom.Configuration(context.Configuration);
    logger.Enrich.FromLogContext();
});

//AutoMapper
builder.Services.AddAutoMapper(p =>
{
    p.AddMaps("EasyWeChat.Service");
});

builder.Services.AddHttpContextAccessor();

//���ģ����֤
builder.Services.AddOptions().Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errorInfo = new ValidationProblemDetails(context.ModelState).Errors
             .Select(t => $"{t.Key}:{string.Join(",", t.Value)}");
        return new OkObjectResult(new ResponseDto
        {
            Code = 404,
            Message = string.Join("\r\n", errorInfo)
        });
    };
});

//ע��dbcontext
builder.Services.AddDbContext<EasyWeChatDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sqlserver")!);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    //app.UseSwaggerUI();
    // �Զ����UI
    app.UseKnife4UI();

}

//�����м��
app.UseErrorHandling();

LocationStorage.Instance = app.Services;

app.UseStaticFiles();

app.UseCors("EasyWeChat.Client");

//��֤
app.UseAuthentication();
//��Ȩ
app.UseAuthorization();

app.MapControllers();

app.Run();
