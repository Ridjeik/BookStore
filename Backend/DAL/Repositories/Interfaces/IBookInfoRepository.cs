using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IBookInfoRepository : IBaseRepository<BookInfo, Guid>
    {
        public IQueryable<BookInfo> GetBooksWithInfoQueryable();
        public Task<BookInfo> GetBookInfoByAuthorAsync(string author);
        public Task<BookInfo> GetBookInfoByTitleAsync(string title);
        Task<IEnumerable<BookInfo>> GetMostPopularBooks(int top);
        Task<IEnumerable<BookInfo>> GetBooksWithInfo();
        Task<IEnumerable<BookInfo>> GetBooksWithInfoAndBooks();
        Task<BookInfo> GetBookWithInfoAndBooks(Guid guid);
    }
}
