using System.Collections.Generic;
using System.Threading.Tasks;
using LinCms.Core.Entities;
using LinCms.Core.EntityQueryParameters;

namespace LinCms.Core.RepositoryInterfaces
{
    public interface IBookRepository
    {
        Task<Book> GetDetailAsync(int id);

        Task<IEnumerable<Book>> GetAllAsync(BookParameters parameters);

        void Add(Book book);

        void Update(Book book);

        void Delete(Book book);
    }
}
