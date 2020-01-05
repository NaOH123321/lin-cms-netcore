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
                            ControllerName = controllerName
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
                            ControllerName = controllerName
                        };
                        permissionMetas.Add(permissionMeta);
                    });
                }
            }

            //判断是否有重复的auth
            if (permissionMetas.GroupBy(i => i.Auth).Where(g => g.Key != null).Any(g => g.Count() > 1))
            {
                throw new Exception("不能有重复的权限名称, 请联系超级管理员");
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
        /// <returns></returns>
        public static List<PermissionMeta> GetMetasByMethod(string methodName)
        {
            var result = new List<PermissionMeta>();

            string? controllerName = null;
            foreach (var controllerType in GetControllerTypes())
            {
                var method = controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .FirstOrDefault(m => m.Name == methodName);
                if (method != null)
                {
                    controllerName = controllerType.FullName;
                    break;
                }
            }
            if (controllerName == null)
            {
                return result;
            }

            result = GetAllMetas().Where(meta => meta.ControllerName == controllerName)
                .Where(meta => meta.MethodName == methodName || meta.MethodName == null).ToList();
            return result;
        }
    }
}
