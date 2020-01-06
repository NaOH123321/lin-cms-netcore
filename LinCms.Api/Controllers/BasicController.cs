using System;
using System.Collections.Generic;
using System.Net.Mime;
using AutoMapper;
using LinCms.Core;
using LinCms.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LinCms.Api.Controllers
{
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ApiController]
    public class BasicController : ControllerBase
    {
        public IMapper MyMapper { set; get; } = null!;
        public IUnitOfWork UnitOfWork { set; get; } = null!;

        public ICurrentUser CurrentUser { set; get; } = null!;

        public PaginatedResult<TR> WrapPaginatedResult<TS, TR>(PaginatedList<TS> list, IEnumerable<TR> resources)
            where TS : class where TR : class
        {
            var result = new PaginatedResult<TR>
            {
                Page = list.Page,
                Count = list.Count,
                TotalPage = list.TotalPage,
                Total = list.Total,
                Items = resources
            };
            return result;
        }
    }
}
