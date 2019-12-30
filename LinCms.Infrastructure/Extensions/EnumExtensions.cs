using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LinCms.Infrastructure.Extensions
{
    /// <summary>
    /// 枚举扩展
    /// </summary>
    public static class EnumExtensions
    {
        private static readonly ConcurrentDictionary<Enum, string> ConcurrentDictionary;

        static EnumExtensions()
        {
            ConcurrentDictionary = new ConcurrentDictionary<Enum, string>();
        }

        private static string GetDisplayName(MemberInfo field)
        {
            var att = Attribute.GetCustomAttribute(field, typeof(DisplayAttribute), false);
            return att == null ? field.Name : ((DisplayAttribute)att).Name;
        }

        /// <summary>
        /// 获取枚举的描述信息(Display.Name)。
        /// 支持位域，如果是位域组合值，多个按分隔符组合。
        /// </summary>
        public static string GetDisplayName(this Enum @this)
        {
            return ConcurrentDictionary.GetOrAdd(@this, (key) =>
            {
                var type = key.GetType();
                var field = type.GetField(key.ToString());
                //如果field为null则应该是组合位域值，
                return field == null ? key.GetDisplayNames() : GetDisplayName(field);
            });
        }

        /// <summary>
        /// 获取枚举的说明
        /// </summary>
        /// <param name="em"></param>
        /// <param name="split">位枚举的分割符号（仅对位枚举有作用）</param>
        /// <returns></returns>
        public static string GetDisplayNames(this Enum em, string split = ",")
        {
            var names = em.ToString().Split(',');
            var res = new string[names.Length];
            var type = em.GetType();
            for (var i = 0; i < names.Length; i++)
            {
                var field = type.GetField(names[i].Trim());
                if (field == null) continue;
                res[i] = GetDisplayName(field);
            }
            return string.Join(split, res);
        }
    }
}
