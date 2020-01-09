using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using LinCms.Api.Exceptions;
using LinCms.Api.Helpers;
using LinCms.Core.Interfaces;
using LinCms.Infrastructure.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace LinCms.Api.Services
{
    public class FileService : IFileService
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public string StoreDir => Path.Combine(_environment.WebRootPath, _configuration["File:StoreDir"]);
        public int SingleLimit => int.Parse(_configuration["File:SingleLimit"]) * 1024 * 1024;
        public int TotalLimit => int.Parse(_configuration["File:TotalLimit"]) * 1024 * 1024;
        public int Num => int.Parse(_configuration["File:Num"]);
        public string[] Includes => _configuration.GetSection("File:Include").Get<string[]>();
        public string[] Excludes => _configuration.GetSection("File:Exclude").Get<string[]>();

        public FileService(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public void Verify(IFormFileCollection files)
        {
            AllowedFile(files);
            AllowedFileSize(files);
        }

        public void AllowedFile(IFormFileCollection files)
        {
            if (Includes != null && Includes.Any())
            {
                if (files.Any(file => !Includes.Contains(FileHelper.GetFileExtension(file).Split(".")[1])))
                {
                    throw new FileExtensionException();
                }
            }

            if (Excludes != null && Excludes.Any())
            {
                if (files.Any(file => Excludes.Contains(FileHelper.GetFileExtension(file).Split(".")[1])))
                {
                    throw new FileExtensionException();
                }
            }
        }

        public void AllowedFileSize(IFormFileCollection files)
        {
            var fileCount = files.Count;
            if (fileCount > Num)
            {
                throw new FileTooManyException();
            }

            var totalSize = 0;
            foreach (var file in files)
            {
                var singleSize = FileHelper.GetFileSize(file);
                if (singleSize > SingleLimit)
                {
                    throw new FileTooLargeException
                    {
                        Msg = $"{file.FileName}大小不能超过{SingleLimit}字节"
                    };
                }

                totalSize += singleSize;
            }

            if (totalSize > TotalLimit)
            {
                throw new FileTooLargeException();
            }
        }

        public (string realName, string relativePath, string absolutePath) GetStorePath(IFormFile file)
        {
            var formDate = DateTime.Now.ToString("yyyyMMdd");
            var saveDir = Path.Combine(StoreDir, formDate);  
            if (!Directory.Exists(saveDir))
            {
                Directory.CreateDirectory(saveDir);
            }

            var realName = FileHelper.GetRealName(file);
            var relativePath = Path.Combine(formDate, realName);
            var absolutePath = Path.Combine(StoreDir, relativePath);

            return (realName, relativePath, absolutePath);
        }

        public void SaveFile(IFormFile file, string absolutePath)
        {
            var fileStream = new FileStream(absolutePath, FileMode.Create);
            file.CopyTo(fileStream);
            fileStream.Flush();
            fileStream.Close();
        }
    }
}