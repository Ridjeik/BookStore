using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IBookRepository : IBaseRepository<Book, Guid>
    {
        IQueryable<Book> GetBooksWithInfoQueryable();
        public Task<IEnumerable<Book>> GetBooksWithInfo();
        public Task<Book?> GetBookWithInfo(Guid id);

        public Task<IEnumerable<Book>> GetBooksByAuthor(Guid authorId);

        public Task<IEnumerable<Book>> GetBooksByPublisher(Guid publisherId);

        public Task<IEnumerable<Book>> GetBooksByGenre(Guid genreId);

        public Task<IEnumerable<Book>> GetBooksByTitle(string title);
        Task<IEnumerable<string>> GetTopBooksNamesWithMostReservations(int count);
    }
}
