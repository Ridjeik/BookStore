using DAL.Repositories.Implementation;
using DAL.Repositories.Interfaces;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Implemantations
{
    public class BookRepository : BaseRepository<Book, Guid>, IBookRepository
    {
        public BookRepository(DbContext context) : base(context)
        {

        }

        public IQueryable<Book> GetBooksWithInfoQueryable()
        {
            return _set
                .Include(b => b.BookInfo)
                .Include(b => b.BookInfo)
                    .ThenInclude(bi => bi.Author)
                .Include(b => b.BookInfo)
                    .ThenInclude(bi => bi.Publisher)
                .Include(b => b.BookInfo)
                    .ThenInclude(bi => bi.Title)
                .Include(b => b.BookInfo)
                    .ThenInclude(bi => bi.Picture)
                .Include(b => b.BookInfo)
                    .ThenInclude(bi => bi.BookGenre)
                        .ThenInclude(bg => bg.Genre)
                .Include(b => b.Reservations)
                    .ThenInclude(r => r.Events)
                .Include(b => b.Reservations)
                .ThenInclude(r => r.Member)
                .ThenInclude(u => u.User)
                .Include(b => b.Reservations)
                .ThenInclude(r => r.Employee)
                .ThenInclude(u => u.User);

        }
        public async Task<IEnumerable<Book>> GetBooksWithInfo()
        {
            return await GetBooksWithInfoQueryable()
                    .ToListAsync();
        }

        public async Task<Book?> GetBookWithInfo(Guid id)
        {
            return await GetBooksWithInfoQueryable()
                    .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Book>> GetBooksByAuthor(Guid authorId)
        {
            return await GetBooksWithInfoQueryable()
                    .Where(b => b.BookInfo.AuthorId == authorId)
                    .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksByPublisher(Guid publisherId)
        {
            return await GetBooksWithInfoQueryable()
                    .Where(b => b.BookInfo.PublisherId == publisherId)
                    .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksByGenre(Guid genreId)
        {
            return await GetBooksWithInfoQueryable()
                    .Where(b => b.BookInfo.BookGenre.Any(st => st.GenreId == genreId))
                    .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksByTitle(string title)
        {
            return await GetBooksWithInfoQueryable()
                    .Where(b => b.BookInfo.Title.Name.Contains(title))
                    .ToListAsync();
        }

        Task<IEnumerable<string>> IBookRepository.GetTopBooksNamesWithMostReservations(int count)
        {
            throw new NotImplementedException();
        }
    }
}
