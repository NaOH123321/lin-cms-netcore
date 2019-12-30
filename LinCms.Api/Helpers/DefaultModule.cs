using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Mvc;

namespace LinCms.Api.Helpers
{
    public class DefaultModule: Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            //获取所有控制器类型并使用属性注入
            var controllerBaseType = typeof(ControllerBase);
            containerBuilder.RegisterAssemblyTypes(typeof(Program).Assembly)
                .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
                .PropertiesAutowired();

            //containerBuilder.RegisterType<XiaoMi>().As<IPhone>().PropertiesAutowired();
        }
    }
}
