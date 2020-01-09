using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using LinCms.Infrastructure.Helpers;
using Microsoft.AspNetCore.Http;

namespace LinCms.Api.Helpers
{
    public class FileHelper
    {
        public static string GetRealName(IFormFile file)
        {
            var realName = Guid.NewGuid() + GetFileExtension(file);
            return realName;
        }

        public static string GetFileExtension(IFormFile file)
        {
            return Path.GetExtension(file.FileName).ToLower();
        }

        public static int GetFileSize(IFormFile file)
        {
            return (int)file.Length;
        }

        public static string GenerateMd5(IFormFile file)
        {
            using var fileStream = file.OpenReadStream();
            var bt = new byte[fileStream.Length];
            fileStream.Read(bt, 0, bt.Length);
            var fileString = Convert.ToBase64String(bt);

            return HashAlgorithmHelper.ComputeHash<MD5>(fileString);
        }
    }
}
