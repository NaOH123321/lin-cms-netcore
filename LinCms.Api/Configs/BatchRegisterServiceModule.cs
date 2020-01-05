using System;
using System.Linq;
using Autofac;
using LinCms.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LinCms.Api.Configs
{
    public class BatchRegisterServiceModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            //获取所有控制器类型并使用属性注入
            var controllerBaseType = typeof(ControllerBase);
            containerBuilder.RegisterAssemblyTypes(typeof(Program).Assembly)
                .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
                .PropertiesAutowired();

            //containerBuilder.RegisterType<XiaoMi>().As<IPhone>().PropertiesAutowired();

            //批量注册校验资源
            var baseValidatorType = typeof(IFluentValidator);
            containerBuilder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies()).Where(b => b.GetInterfaces().
                    Any(c => c == baseValidatorType && b != baseValidatorType)).AsImplementedInterfaces().InstancePerDependency();

            //批量注册数据仓库
            var baseRepositoryType = typeof(IRepository);
            containerBuilder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies()).Where(b => b.GetInterfaces().
                Any(c => c == baseRepositoryType && b != baseRepositoryType)).AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}
