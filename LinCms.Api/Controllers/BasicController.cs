using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using LinCms.Api.Exceptions;
using LinCms.Core.Entities;
using LinCms.Core.Interfaces;
using LinCms.Infrastructure.Messages;
using Microsoft.AspNetCore.Mvc;

namespace LinCms.Api.Controllers
{
    [ApiController]
    public class BasicController : ControllerBase
    {
        public IMapper MyMapper { set; get; } = null!;
        public IUnitOfWork UnitOfWork { set; get; } = null!;

        public ICurrentUser CurrentUser { set; get; } = null!;
    }
}
