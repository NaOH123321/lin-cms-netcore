using System;
using System.Collections.Generic;
using System.Text;

namespace LinCms.Core.Interfaces
{
    public interface ILinLogger
    {
        void AddLog(int id, string? userName, string template, string? auth);
    }
}
