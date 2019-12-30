using System;
using System.Collections.Generic;
using System.Text;
using LinCms.Core.Interfaces;

namespace LinCms.Core.Entities
{
    public class LinLog : IEntityInt
    {
        public int Id { get; set; }
        public string? Message { get; set; }
        public DateTime? Time { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public int? StatusCode { get; set; }
        public string? Method { get; set; }
        public string? Path { get; set; }
        public string? Authority { get; set; }
    }
}
