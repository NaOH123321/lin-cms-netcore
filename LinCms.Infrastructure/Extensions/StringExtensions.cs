using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace LinCms.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string ToSnakeCase(this string input)
        {
            return Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }
    }
}
