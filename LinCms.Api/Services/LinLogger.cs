using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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


        private readonly List<string> _templateObj = new List<string>
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
            var message = TemplateReplace(template);
            var httpContext = _httpContextAccessor.HttpContext;

            var log = new LinLog
            {
                UserId = id,
                UserName = userName,
                Method = httpContext.Request.Method,
                Path = httpContext.Request.Path,
                StatusCode = httpContext.Response.StatusCode,
                Time = DateTime.Now,
                Message = message,
                Authority = auth
            };

            _linContext.Add(log);
            _unitOfWork.Save();
        }

        private string TemplateReplace(string template)
        {
            var currentUser = (ICurrentUser) _httpContextAccessor.HttpContext.RequestServices.GetService(typeof(ICurrentUser));
            if (currentUser.Username == null) return template;

            var logTemplates = LinLogTemplate.TemplateParser(template);

            var newTemplate = template;
            foreach (var logTemplate in logTemplates)
            {
                if (!_templateObj.Contains(logTemplate.ClassObj))
                {
                    continue;
                }

                string? item = null;
                switch (logTemplate.ClassObj)
                {
                    case "user":
                        var propertyInfo = typeof(CurrentUser).GetProperty(logTemplate.Property,
                            BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
                        item = propertyInfo?.GetValue(currentUser)?.ToString();
                        break;
                    case "response":
                        item = "ss";
                        break;
                    default:
                        item = "ss";
                        break;
                }

                if (item == null) continue;

                newTemplate = newTemplate.Replace(logTemplate.Segment, item);
            }

            return newTemplate;
        }
    }

    public class LinLogTemplate
    {        
        private const string Pattern = "[{](.*?)[}]";
        private const string SeparatorString = ".";

        public string ClassObj { get; }
        public string Property { get; }
        public string Value { get; }
        public string Segment { get; }

        private LinLogTemplate(string segment)
        {
            Segment = segment;
            Value = Segment.Replace("{", "").Replace("}", "");
            ClassObj = Value.Split(SeparatorString)[0];
            Property = Value.Split(SeparatorString)[1];
        }

        public static IEnumerable<LinLogTemplate> TemplateParser(string template)
        {
            var matches = Regex.Matches(template, Pattern);
            var segments = matches.Select(m => m.Value).ToList();
            var logTemplates = new List<LinLogTemplate>();
            foreach (var segment in segments)
            {
                try
                {
                    var logTemplate = new LinLogTemplate(segment);
                    logTemplates.Add(logTemplate);
                }
                catch (Exception)
                {
                    continue;
                }
            }

            return logTemplates;
        }
    }
}
