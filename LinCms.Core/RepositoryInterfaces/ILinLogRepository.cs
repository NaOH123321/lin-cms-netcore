using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LinCms.Core.Entities;
using LinCms.Core.EntityQueryParameters;

namespace LinCms.Core.RepositoryInterfaces
{
    public interface ILinLogRepository
    {
        Task<PaginatedList<LinLog>> GetAllLogsAsync(LogParameters logParameters);
    }
}
