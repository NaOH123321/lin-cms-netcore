using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LinCms.Core;
using Microsoft.AspNetCore.Mvc;

namespace LinCms.Api.Helpers
{
    public class PermissionMetaHandler
    {
        private static IEnumerable<Type> GetControllerTypes()
        {
            var controllerBaseType = typeof(ControllerBase);
            var controllerTypes = typeof(Startup).Assembly.GetTypes()
                .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType).ToList();

            return controllerTypes;
        }

        /// <summary>
        /// 获取所有继承于ControllerBase的Controller的meta
        /// </summary>
        /// <returns></returns>
        public static List<PermissionMeta> GetAllMetas()
        {
            var permissionMetas = new List<PermissionMeta>();

            foreach (var controllerType in GetControllerTypes())
            {
                var controllerName = controllerType.FullName;

                var routeAttributes = controllerType.GetCustomAttributes<RouteAttribute>().ToList();
                routeAttributes.ForEach(route =>
                {
                    var routeName = route.Template.Replace("/", ".");
                    permissionMetas.AddRange(GetAllByControllerRoute(controllerType, routeName));
                });
            }

            return permissionMetas;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controllerType">controller的类型</param>
        /// <param name="routeName">controller的路由名称，例如：cms.admin</param>
        /// <returns></returns>
        private static List<PermissionMeta> GetAllByControllerRoute(Type controllerType, string routeName)
        {
            var permissionMetas = new List<PermissionMeta>();

            var controllerPermissionMetaAttributes = controllerType.GetCustomAttributes<PermissionMetaAttribute>().ToList();
            if (controllerPermissionMetaAttributes.Any())
            {
                controllerPermissionMetaAttributes.ForEach(permissionMetaAttribute =>
                {
                    var permissionMeta = new PermissionMeta
                    {
                        Auth = permissionMetaAttribute.Auth,
                        Module = permissionMetaAttribute.Module,
                        Role = permissionMetaAttribute.Role,
                        Mount = permissionMetaAttribute.Mount,
                        RouteName = routeName
                    };
                    permissionMetas.Add(permissionMeta);
                });
            }

            var methods = controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            foreach (var method in methods)
            {
                var permissionMetaAttributes = method.GetCustomAttributes<PermissionMetaAttribute>().ToList();
                if (!permissionMetaAttributes.Any()) continue;

                permissionMetaAttributes.ForEach(permissionMetaAttribute =>
                {
                    var permissionMeta = new PermissionMeta
                    {
                        Auth = permissionMetaAttribute.Auth,
                        Module = permissionMetaAttribute.Module,
                        Role = permissionMetaAttribute.Role,
                        Mount = permissionMetaAttribute.Mount,
                        MethodName = method.Name,
                        RouteName = routeName
                    };
                    permissionMetas.Add(permissionMeta);
                });
            }

            return permissionMetas;
        }

        /// <summary>
        /// 获取所有可分配的meta
        /// </summary>
        /// <returns></returns>
        public static List<PermissionMeta> GetAllDispatchedMetas()
        {
            return GetAllMetas().Where(meta => meta.Mount).ToList();
        }

        /// <summary>
        /// 获取所有对这个方法有影响的权限meta
        /// </summary>
        /// <param name="methodName">路由的方法名(不是路由的路径)</param>
        /// <param name="routeName">controller的路由名称，例如：cms.admin(需用.分隔)</param>
        /// <returns></returns>
        public static List<PermissionMeta> GetMetasByMethod(string methodName, string routeName)
        {
            //var result = new List<PermissionMeta>();

            //string? controllerName = null;
            //foreach (var controllerType in GetControllerTypes())
            //{
            //    var method = controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
            //        .FirstOrDefault(m => m.Name == methodName);
            //    if (method != null)
            //    {
            //        controllerName = controllerType.FullName;
            //        break;
            //    }
            //}
            //if (controllerName == null)
            //{
            //    return result;
            //}

            var result = GetAllMetas().Where(meta => meta.RouteName == routeName)
                .Where(meta => meta.MethodName == methodName || meta.MethodName == null).ToList();
            return result;
        }
    }
}
