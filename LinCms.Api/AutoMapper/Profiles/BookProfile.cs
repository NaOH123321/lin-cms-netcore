using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LinCms.Core.Entities;
using LinCms.Infrastructure.Resources;

namespace LinCms.Api.AutoMapper.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookResource>();

            CreateMap<BookAddResource, Book>();
            CreateMap<BookUpdateResource, Book>();
        }
    }
}
