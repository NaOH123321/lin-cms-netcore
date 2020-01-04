using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinCms.Core;
using LinCms.Core.Entities;
using LinCms.Core.EntityQueryParameters;
using LinCms.Core.RepositoryInterfaces;
using LinCms.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace LinCms.Infrastructure.Repositories
{
    public class LinLogRepository : ILinLogRepository
    {
        private readonly LinContext _linContext;

        public LinLogRepository(LinContext linContext)
        {
            _linContext = linContext;
        }

        public async Task<PaginatedList<LinLog>> GetAllLogsAsync(LogParameters logParameters)
        {
            var query = _linContext.LinLogs
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(logParameters.Name))
            {
                query = query.Where(u => u.UserName == logParameters.Name);
            }

            if (logParameters.StartTime != null)
            {
                query = query.Where(l =>
                    l.Time != null &&
                    DateTime.Compare((DateTime) l.Time, (DateTime) logParameters.StartTime) > 0);
            }

            if (logParameters.EndTime != null)
            {
                //判断查询是否有时间，没有则包括当天
                var endTime = (DateTime) logParameters.EndTime;
                if (endTime.TimeOfDay.Ticks == 0)
                {
                    endTime = endTime.AddDays(1);
                }

                query = query.Where(l =>
                    l.Time != null &&
                    DateTime.Compare((DateTime) l.Time, endTime) < 0);
            }

            query = query.OrderByDescending(l => l.Time);

            var total = await query.CountAsync();
            var data = await query
                .Skip(logParameters.Start + logParameters.Page * logParameters.Count)
                .Take(logParameters.Count)
                .ToListAsync();

            return new PaginatedList<LinLog>(logParameters.Page, logParameters.Count, total, data);
        }
    }
}
