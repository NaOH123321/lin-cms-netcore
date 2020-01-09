using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace LinCms.Core.Interfaces
{
    public interface IFileService
    {
        string StoreDir { get; }
        int SingleLimit { get; }
        int TotalLimit { get; }
        int Num { get; }
        string[] Includes { get; }
        string[] Excludes { get; }

        (string realName, string relativePath, string absolutePath) GetStorePath(IFormFile file);

        void SaveFile(IFormFile file, string absolutePath);

        void Verify(IFormFileCollection files);

        void AllowedFile(IFormFileCollection files);
        void AllowedFileSize(IFormFileCollection files);
    }
}