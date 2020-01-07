using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using LinCms.Api.Exceptions;
using LinCms.Api.Helpers;
using LinCms.Api.Services;
using LinCms.Core;
using LinCms.Core.Entities;
using LinCms.Core.RepositoryInterfaces;
using LinCms.Infrastructure.Repositories;
using LinCms.Infrastructure.Resources.LinUsers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LinCms.Api.Controllers.Cms
{
    //[Authorize]
    [Route("cms/file")]
    public class FileController : BasicController
    {
        private readonly FileService _fileService;
        private readonly ILinFileRepository _linFileRepository;

        public FileController(FileService fileService, ILinFileRepository linFileRepository)
        {
            _fileService = fileService;
            _linFileRepository = linFileRepository;
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<FileResource>>> UploadFile([FromForm] IFormFileCollection formFileCollection)
        {

            if (formFileCollection.Count == 0)
            {
                throw new BadRequestException
                {
                    Msg = "文件不能为空"
                };
            }

            var fileResults = new List<FileResource>();

            foreach (var file in formFileCollection)
            {
                var storePath = GetStorePath(file);

                var linFile = new LinFile
                {
                    Name = storePath.realName,
                    Path = storePath.relativePath,
                    Extension = GetFileExtension(file),
                    Size = GetFileSize(file),
                    Md5 = GenerateMd5(file)
                };

                SaveFile(file, storePath.absolutePath);



                _linFileRepository.Add(linFile);

                var fileResult = new FileResource
                {
                    Key = file.Name,
                    Id = linFile.Id,
                    Path = linFile.Path,
                    Url = Path.Combine(_fileService.StoreDir, linFile.Path)
                };
                fileResults.Add(fileResult);
            }


            if (!await UnitOfWork.SaveAsync())
            {
                throw new Exception("Save Failed!");
            }

            return Ok(fileResults);
        }


        public (string realName, string relativePath, string absolutePath) GetStorePath(IFormFile file)
        {
            var formDate = DateTime.Now.ToString("yyyyMMdd");
            var saveDir = _fileService.StoreDir + $"{formDate}";
            if (!Directory.Exists(saveDir))
            {
                Directory.CreateDirectory(saveDir);
            }

            var realName = GetRealName(file);
            var relativePath = $@"{formDate}\{realName}";
            var absolutePath = Path.Combine(_fileService.StoreDir, relativePath);

            return (realName, relativePath, absolutePath);
        }

        public static string GetRealName(IFormFile file)
        {
            var realName = Guid.NewGuid() + GetFileExtension(file);
            return realName;
        }

        public static void SaveFile(IFormFile file, string absolutePath)
        {
            using var fileStream = new FileStream(absolutePath, FileMode.Create);
            file.CopyTo(fileStream);
            fileStream.Flush();
        }

        public static string GetFileExtension(IFormFile file)
        {
            return Path.GetExtension(file.FileName);
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

    public class FileResource
    {
        public string Key { get; set; } = null!;
        public int Id { get; set; }
        public string Path { get; set; } = null!;
        public string Url { get; set; } = null!;
    }
}
