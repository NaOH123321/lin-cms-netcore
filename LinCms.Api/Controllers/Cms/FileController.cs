using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using LinCms.Api.Exceptions;
using LinCms.Api.Helpers;
using LinCms.Api.Services;
using LinCms.Core.Entities;
using LinCms.Core.Interfaces;
using LinCms.Core.RepositoryInterfaces;
using LinCms.Infrastructure.Resources.LinFiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace LinCms.Api.Controllers.Cms
{
    [Authorize]
    [Route("cms/file")]
    public class FileController : BasicController
    {
        private readonly IFileService _fileService;
        private readonly ILinFileRepository _linFileRepository;

        public FileController(IFileService fileService, ILinFileRepository linFileRepository)
        {
            _fileService = fileService;
            _linFileRepository = linFileRepository;
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<LinFileResource>>> UploadFile([FromForm] IFormFileCollection files)
        {
            _fileService.Verify(files);

            var fileResults = new List<LinFileResource>();

            foreach (var file in files)
            {
                var resource = await HandleFile(file);

                fileResults.Add(resource);
            }

            return Ok(fileResults);
        }

        private async Task<LinFileResource> HandleFile(IFormFile file)
        {
            var md5 = FileHelper.GenerateMd5(file);
            var linFile = await _linFileRepository.GetFileByMd5(md5);

            if (linFile == null)
            {
                var (realName, relativePath, absolutePath) = _fileService.GetStorePath(file);

                linFile = new LinFile
                {
                    Name = realName,
                    Path = relativePath,
                    Extension = FileHelper.GetFileExtension(file),
                    Size = FileHelper.GetFileSize(file),
                    Md5 = md5
                };

                _fileService.SaveFile(file, absolutePath);
                _linFileRepository.Add(linFile);
                await UnitOfWork.SaveAsync();
            }

            var resource = new LinFileResource
            {
                Key = file.Name,
                Id = linFile.Id,
                Path = linFile.Path,
                Url = Path.Combine(_fileService.StoreDir, linFile.Path)
            };
            return resource;
        }
    }
}
