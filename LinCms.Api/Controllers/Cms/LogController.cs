using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinCms.Api.Helpers;
using LinCms.Core;
using LinCms.Core.Entities;
using LinCms.Core.EntityQueryParameters;
using LinCms.Core.RepositoryInterfaces;
using LinCms.Infrastructure.Resources.LinGroups;
using LinCms.Infrastructure.Resources.LinLogs;
using Microsoft.AspNetCore.Mvc;

namespace LinCms.Api.Controllers.Cms
{
    [Route("cms/log")]
    public class LogController : BasicController
    {
        private readonly ILinLogRepository _linLogRepository;

        public LogController(ILinLogRepository linLogRepository)
        {
            _linLogRepository = linLogRepository;
        }

        [HttpGet]
        [PermissionMeta("查询所有日志", "日志")]
        public async Task<ActionResult<PaginatedResult<LinLogResource>>> GetAllLogs(
            [FromQuery] LogParameters logParameters)
        {
            var list = await _linLogRepository.GetAllLogsAsync(logParameters);

            var resources = MyMapper.Map<IEnumerable<LinLog>, IEnumerable<LinLogResource>>(list);

            var result = WrapPaginatedResult(list, resources);

            return Ok(result);
        }
    }
}
