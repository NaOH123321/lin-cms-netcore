﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinCms.Api.Exceptions;
using LinCms.Api.Helpers;
using LinCms.Core;
using LinCms.Core.Entities;
using LinCms.Core.EntityQueryParameters;
using LinCms.Core.RepositoryInterfaces;
using LinCms.Infrastructure.Messages;
using LinCms.Infrastructure.Resources;
using LinCms.Infrastructure.Resources.Books;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LinCms.Api.Controllers.V1
{
    [Route("v1/book")]
    public class BookController : BasicController
    {
        private readonly IBookRepository _bookRepository;

        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [Authorize]
        [HttpGet(Name = "GetBooks")]
        public async Task<ActionResult<IEnumerable<BookResource>>> GetAll([FromQuery] BookParameters parameters)
        {
            var books = await _bookRepository.GetAllAsync(parameters);
            var resources = MyMapper.Map<IEnumerable<BookResource>>(books);

            return Ok(resources);
        }

        [Authorize]
        [HttpGet("{id}", Name = "GetBook")]
        public async Task<ActionResult<BookResource>> Get(int id)
        {
            var book = await _bookRepository.GetDetailAsync(id);
            if (book == null)
            {
                throw new BookNotFoundException();
            }

            var resource = MyMapper.Map<BookResource>(book);
            return Ok(resource);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost(Name = "AddBook")]
        public async Task<ActionResult<BookResource>> Post(BookAddResource bookAddResource)
        {
            var book = MyMapper.Map<BookAddResource, Book>(bookAddResource);

            _bookRepository.Add(book);

            if (!await UnitOfWork.SaveAsync())
            {
                throw new Exception("Save Failed!");
            }

            var resource = MyMapper.Map<BookResource>(book);
            return CreatedAtRoute("GetBook", new { id = resource.Id }, resource);
        }

        [HttpPut("{id}", Name = "UpdateBook")]
        public async Task<ActionResult<BookResource>> Put(int id, BookUpdateResource bookUpdateResource)
        {
            var book = await _bookRepository.GetDetailAsync(id);
            if (book == null)
            {
                throw new BookNotFoundException();
            }

            MyMapper.Map(bookUpdateResource, book);

            _bookRepository.Update(book);

            if (!await UnitOfWork.SaveAsync())
            {
                throw new Exception("Save Failed!");
            }

            var resource = MyMapper.Map<BookResource>(book);
            return Ok(resource);
        }

        [PermissionMeta("删除图书", "图书")]
        [HttpDelete("{id}",Name = "DeleteBook")]
        public async Task<ActionResult<OkMsg>> Delete(int id)
        {
            var book = await _bookRepository.GetDetailAsync(id);
            if (book == null)
            {
                throw new BookNotFoundException();
            }

            _bookRepository.Delete(book);

            if (!await UnitOfWork.SaveAsync())
            {
                throw new Exception("Save Failed!");
            }

            return Ok(new OkMsg
            {
                Msg = "删除图书成功"
            });
        }
    }
}
