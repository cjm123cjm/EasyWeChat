using Autofac;
using Autofac.Extras.DynamicProxy;
using EasyWeChat.Domain.IRepository;
using EasyWeChat.Domain.Repository;
using EasyWeChat.IService.Interfaces;
using EasyWeChat.Service.Implement;
using System.Reflection;

namespace EasyWeChat.Api.Extensions
{
    /// <summary>
    /// autofac
    /// </summary>
    public class AutofacModuleRegister : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<JwtTokenGenerator>().As<IJwtTokenGenerator>().InstancePerLifetimeScope();

            var aopType = new List<Type> { typeof(ServiceAop) };
            builder.RegisterType<ServiceAop>();

            //获取 Service.dll 程序集服务,并注册
            builder.RegisterAssemblyTypes(Assembly.Load("EasyWeChat.IService"), Assembly.Load("EasyWeChat.Service"))
                .Where(a => a.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerDependency()
                .EnableInterfaceInterceptors()
                .InterceptedBy(aopType.ToArray());

            builder.RegisterGeneric(typeof(RepositoryBase<>)).As(typeof(IRepositoryBase<>)); //注册仓储

            //获取 Repository.dll 程序集服务,并注册
            builder.RegisterAssemblyTypes(Assembly.Load("EasyWeChat.Domain"))
                .Where(a => a.Name.EndsWith("Repository"))
                .AsImplementedInterfaces();
        }
    }

}
