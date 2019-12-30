using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LinCms.Api.Helpers
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class RequestHeaderMatchingMediaTypeAttribute : Attribute, IActionConstraint
    {
        private readonly string _requestHeaderToMatch;
        private readonly string[] _mediaTypes;

        public RequestHeaderMatchingMediaTypeAttribute(string requestHeaderToMatch, string[] mediaTypes)
        {
            _requestHeaderToMatch = requestHeaderToMatch;
            _mediaTypes = mediaTypes;
        }

        public bool Accept(ActionConstraintContext context)
        {
            var requestHeaders = context.RouteContext.HttpContext.Request.Headers;
            if (!requestHeaders.ContainsKey(_requestHeaderToMatch))
            {
                return false;
            }

            foreach (var mediaType in _mediaTypes)
            {
                var mediaTypeMatches = string.Equals(requestHeaders[_requestHeaderToMatch].ToString(), mediaType, StringComparison.OrdinalIgnoreCase);
                if (mediaTypeMatches) return true;
            }

            return false;
        }

        public int Order { get; } = 0;
    }
}
