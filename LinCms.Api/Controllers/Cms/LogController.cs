using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinCms.Api.Exceptions;
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

        [HttpGet("search")]
        [PermissionMeta("搜索日志", "日志")]
        public async Task<ActionResult<PaginatedResult<LinLogResource>>> SearchForLog(
            [FromQuery] SearchLogParameters searchLogParameters)
        {
            if (string.IsNullOrWhiteSpace(searchLogParameters.Keyword))
            {
                throw new BadRequestException
                {
                    Msg = "搜索关键字不可为空"
                };
            }

            var list = await _linLogRepository.SearchAllLogsAsync(searchLogParameters);

            var resources = MyMapper.Map<IEnumerable<LinLog>, IEnumerable<LinLogResource>>(list);

            var result = WrapPaginatedResult(list, resources);

            return Ok(result);
        }

        [HttpGet("users")]
        [PermissionMeta("查询日志记录的用户", "日志")]
        public async Task<ActionResult<PaginatedResult<string>>> GetUsers([FromQuery] EntityQueryParameters parameters)
        {
            var list = await _linLogRepository.GetUsersByLogsAsync(parameters);

            var result = WrapPaginatedResult(list, list);

            return Ok(result);
        }
    }
}
