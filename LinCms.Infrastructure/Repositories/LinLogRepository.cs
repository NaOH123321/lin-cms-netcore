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

            query = BuildQueryForLinLogs(logParameters, query);

            var total = await query.CountAsync();
            var data = await query
                .Skip(logParameters.Start + logParameters.Page * logParameters.Count)
                .Take(logParameters.Count)
                .ToListAsync();

            return new PaginatedList<LinLog>(logParameters.Page, logParameters.Count, total, data);
        }

        public async Task<PaginatedList<LinLog>> SearchAllLogsAsync(SearchLogParameters searchLogParameters)
        {
            var query = _linContext.LinLogs
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchLogParameters.Keyword))
            {
                query = query.Where(u => u.Message != null && u.Message.Contains(searchLogParameters.Keyword));
            }

            query = BuildQueryForLinLogs(searchLogParameters, query);

            var total = await query.CountAsync();
            var data = await query
                .Skip(searchLogParameters.Start + searchLogParameters.Page * searchLogParameters.Count)
                .Take(searchLogParameters.Count)
                .ToListAsync();

            return new PaginatedList<LinLog>(searchLogParameters.Page, searchLogParameters.Count, total, data);
        }

        public async Task<PaginatedList<string>> GetUsersByLogsAsync(QueryParameters parameters)
        {
            var query = _linContext.LinLogs
                .AsQueryable();

            var userNameQuery = query.GroupBy(l => l.UserName).Select(g => g.Key).Where(k => k != null);

            var total = await userNameQuery.CountAsync();
            var data = await userNameQuery
                .Skip(parameters.Start + parameters.Page * parameters.Count)
                .Take(parameters.Count)
                .ToListAsync();

            return new PaginatedList<string>(parameters.Page, parameters.Count, total, data);
        }

        private static IQueryable<LinLog> BuildQueryForLinLogs(LogParameters logParameters, IQueryable<LinLog> query)
        {
            if (!string.IsNullOrWhiteSpace(logParameters.Name))
            {
                query = query.Where(u => u.UserName == logParameters.Name);
            }

            if (logParameters.StartTime != null)
            {
                query = query.Where(l =>
                    l.Time != null &&
                    DateTime.Compare((DateTime)l.Time, (DateTime)logParameters.StartTime) > 0);
            }

            if (logParameters.EndTime != null)
            {
                //判断查询是否有时间，没有则包括当天
                var endTime = (DateTime)logParameters.EndTime;
                if (endTime.TimeOfDay.Ticks == 0)
                {
                    endTime = endTime.AddDays(1);
                }

                query = query.Where(l =>
                    l.Time != null &&
                    DateTime.Compare((DateTime)l.Time, endTime) < 0);
            }

            query = query.OrderByDescending(l => l.Time);

            return query;
        }
    }
}
