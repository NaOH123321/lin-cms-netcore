using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace LinCms.Api.Services
{
    public class FileService
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public string StoreDir => Path.Combine(_environment.WebRootPath) + _configuration["File:StoreDir"];
        public int SingleLimit => int.Parse(_configuration["File:SingleLimit"]);
        public int TotalLimit => int.Parse(_configuration["File:TotalLimit"]);
        public int Num => int.Parse(_configuration["File:TotalLimit"]);
        public string[] Includes => _configuration.GetSection("File:Include").Get<string[]>();
        public string[] Excludes => _configuration.GetSection("File:Exclude").Get<string[]>();

        public FileService(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }



    }
}