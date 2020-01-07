using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace LinCms.Infrastructure.Helpers
{
    public static class HashAlgorithmInstances<THashAlgorithm> where THashAlgorithm : HashAlgorithm
    {
        /// <summary>
        /// 线程静态变量。
        /// 即：这个变量在每个线程中都是唯一的。
        /// 再结合泛型类实现了该变量在不同泛型或不同的线程先的变量都是唯一的。
        /// 这样做的目的是为了避开多线程问题。
        /// </summary>
        [ThreadStatic]
        private static THashAlgorithm _instance;

        public static THashAlgorithm Instance => _instance ?? Create(); // C# 语法糖，低版本可以改为 { get { return instance != null ? instance : Create(); } }

        /// <summary>
        /// 寻找 THashAlgorithm 类型下的 Create 静态方法，并执行它。
        /// 如果没找到，则执行 Activator.CreateInstance 调用构造方法创建实例。
        /// 如果 Activator.CreateInstance 方法执行失败，它会抛出异常。
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static THashAlgorithm Create()
        {
            var createMethod = typeof(THashAlgorithm).GetMethod(
                nameof(HashAlgorithm.Create), // 这段代码同 "Create"，低版本 C# 可以替换掉
                BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly,
                Type.DefaultBinder,
                Type.EmptyTypes,
                null);

            if (createMethod != null)
            {
                _instance = (THashAlgorithm)createMethod.Invoke(null, new object[] { })!;
            }
            else
            {
                _instance = Activator.CreateInstance<THashAlgorithm>();
            }

            return _instance;
        }
    }

    public static class HashAlgorithmHelper
    {
        private static readonly char[] Digitals = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };

        private static string ToString(byte[] bytes)
        {
            const int byteLen = 2; // 表示一个 byte 的字符长度。

            var chars = new char[byteLen * bytes.Length];

            var index = 0;

            foreach (var item in bytes)
            {
                chars[index] = Digitals[item >> 4/* byte high */]; ++index;
                chars[index] = Digitals[item & 15/* byte low  */]; ++index;
            }

            return new string(chars);
        }

        public static string ComputeHash<THashAlgorithm>(string input) where THashAlgorithm : HashAlgorithm
        {
            var bytes = Encoding.UTF8.GetBytes(input);

            return ToString(HashAlgorithmInstances<THashAlgorithm>.Instance.ComputeHash(bytes));
        }
    }
}
