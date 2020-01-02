using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LinCms.Core.Entities;
using LinCms.Core.Interfaces;
using LinCms.Infrastructure.Database;
using Microsoft.AspNetCore.Http;

namespace LinCms.Api.Services
{
    public class LinLogger : ILinLogger
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LinContext _linContext;
        private readonly IUnitOfWork _unitOfWork;

        private const string Pattern = "[{](.*?)[}]";
        private const string SeparatorString = ".";

        private List<string> _templateObj = new List<string>
        {
            "user", "response", "request"
        };

        public LinLogger(IHttpContextAccessor httpContextAccessor, LinContext linContext, IUnitOfWork unitOfWork)
        {
            _httpContextAccessor = httpContextAccessor;
            _linContext = linContext;
            _unitOfWork = unitOfWork;
        }

        public void AddLog(int id, string? userName, string template, string? auth)
        {
            var matches = Regex.Matches(template, Pattern);

            var sd = matches.Select(m=>m).ToList();
            var s = sd.First().Value;

            var sdss = s.Split(SeparatorString);
            var f11 = sdss[0];
            var f22 = sdss[1];



            var httpContext = _httpContextAccessor.HttpContext;

            var log = new LinLog
            {
                UserId = id,
                UserName = userName,
                Method = httpContext.Request.Method,
                Path = httpContext.Request.Path,
                StatusCode = httpContext.Response.StatusCode,
                Time = DateTime.Now,
                Message = template,
                Authority = auth
            };

            //_linContext.Add(log);

            //_unitOfWork.Save();
        }
    }
}
