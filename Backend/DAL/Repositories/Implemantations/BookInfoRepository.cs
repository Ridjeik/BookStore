using DAL.Repositories.Implementation;
using DAL.Repositories.Interfaces;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Implemantations
{
    public class BookInfoRepository : BaseRepository<BookInfo, Guid>, IBookInfoRepository
    {
        public BookInfoRepository(DbContext context) : base(context)
        {

        }
        public IQueryable<BookInfo> GetBooksWithInfoQueryable()
        {
            return _set
                .Include(b => b.Books)
                .Include(b => b.Author)
                .Include(b => b.Picture)
                .Include(b => b.Publisher)
                .Include(b => b.Title)
                .Include(b => b.BookGenre)
                    .ThenInclude(bg => bg.Genre);
        }

        public async Task<IEnumerable<BookInfo>> GetBooksWithInfoAndBooks()
        {
            return await GetBooksWithInfoQueryable()
                .Include(bi => bi.Books)
                .Where(bi => bi.Books.Any())
                .ToListAsync();
        }

        public async Task<BookInfo> GetBookWithInfoAndBooks(Guid guid)
        {
            return await GetBooksWithInfoQueryable()
                .Include(bi => bi.Books)
                .FirstOrDefaultAsync(bi => bi.Id == guid);
        }

        public async Task<IEnumerable<BookInfo>> GetBooksWithInfo()
        {
            return await GetBooksWithInfoQueryable()
                    .ToListAsync();
        }

        public async Task<BookInfo> GetBookInfoByAuthorAsync(string author)
        {
            return await GetBooksWithInfoQueryable().FirstOrDefaultAsync(bi => bi.Author.Name == author);
        }

        public async Task<BookInfo> GetBookInfoByTitleAsync(string title)
        {
            return await GetBooksWithInfoQueryable().FirstOrDefaultAsync(bi => bi.Title.Name == title);
        }

        public async Task<IEnumerable<BookInfo>> GetMostPopularBooks(int top)
        {
            var popularBooks = new List<Guid>();

            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "GetMostPopularBooks";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@top", top));

                await _context.Database.OpenConnectionAsync();

                using (var result = await command.ExecuteReaderAsync())
                {
                    while (await result.ReadAsync())
                    {
                        popularBooks.Add(result.GetGuid(0));
                    }
                }
            }

            var bookInfos = await GetBooksWithInfoQueryable().Include(a => a.Books).ThenInclude(a => a.Reservations).Include(a => a.Title)
                .Where(bi => popularBooks.Contains(bi.Id))
                .ToListAsync();

            return bookInfos;
        }

    }
}
