using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinCms.Core.Entities;
using LinCms.Core.EntityQueryParameters;
using LinCms.Core.RepositoryInterfaces;
using LinCms.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LinCms.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly LinContext _linContext;

        public BookRepository(LinContext linContext)
        {
            _linContext = linContext;
        }

        public async Task<Book?> GetDetailAsync(int id)
        {
            var query = _linContext.Books
                .AsQueryable();

            query = query.Where(b => b.Id == id);

            var book = await query.SingleOrDefaultAsync();
            return book;
        }

        public async Task<IEnumerable<Book>> GetAllAsync(BookParameters parameters)
        {
            var query = _linContext.Books
                .AsQueryable();

            if (!string.IsNullOrEmpty(parameters.Title))
            {
                query = query.Where(b => b.Title.Contains(parameters.Title));
            }

            var books = await query.ToListAsync();
            return books;
        }

        public void Add(Book book)
        {
            _linContext.Add(book);
        }
        public void Update(Book book)
        {
            _linContext.Update(book);
        }

        public void Delete(Book book)
        {
            _linContext.Remove(book);
        }
    }
}
