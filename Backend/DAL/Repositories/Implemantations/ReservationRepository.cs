using DAL.Repositories.Implementation;
using DAL.Repositories.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Implemantations
{
    public class ReservationRepository : BaseRepository<Reservation, Guid>, IReservationRepository
    {
        public ReservationRepository(DbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByMemberId(string userId)
        {
            return await _set
                .Where(r => r.MemberId == userId)
                .Include(r => r.Events)
                .Include(r => r.Book)
                    .ThenInclude(Book => Book.BookInfo)
                        .ThenInclude(BookInfo => BookInfo.Title)
                .Include(r => r.Book)
                    .ThenInclude(Book => Book.BookInfo)
                        .ThenInclude(BookInfo => BookInfo.Author)
                .Include(r => r.Book)
                    .ThenInclude(Book => Book.BookInfo)
                        .ThenInclude(BookInfo => BookInfo.Publisher)
                .Include(r => r.Book)
                    .ThenInclude(Book => Book.BookInfo)
                        .ThenInclude(BookInfo => BookInfo.Picture)
                .Include(r => r.Book)
                    .ThenInclude(Book => Book.BookInfo)
                        .ThenInclude(BookInfo => BookInfo.BookGenre)
                            .ThenInclude(BookGenre => BookGenre.Genre)
                .OrderByDescending(r => r.Events.Max(e => e.Date))
                .ToListAsync();
        }


        public async Task<IEnumerable<Reservation>> GetReservationsByEmployeeId(string userId)
        {
            return await _set
                .Where(r => r.EmployeeId == userId)
                .Include(r => r.Events)
                .Include(r => r.Book)
                    .ThenInclude(Book => Book.BookInfo)
                        .ThenInclude(BookInfo => BookInfo.Title)
                .Include(r => r.Book)
                    .ThenInclude(Book => Book.BookInfo)
                        .ThenInclude(BookInfo => BookInfo.Author)
                .Include(r => r.Book)
                    .ThenInclude(Book => Book.BookInfo)
                        .ThenInclude(BookInfo => BookInfo.Publisher)
                .Include(r => r.Book)
                    .ThenInclude(Book => Book.BookInfo)
                        .ThenInclude(BookInfo => BookInfo.Picture)
                .Include(r => r.Book)
                    .ThenInclude(Book => Book.BookInfo)
                        .ThenInclude(BookInfo => BookInfo.BookGenre)
                            .ThenInclude(BookGenre => BookGenre.Genre)
                .OrderByDescending(r => r.Events.Max(e => e.Date))
                .ToListAsync();
        }

        public async Task<Reservation> GetLatestReservationByBookId(Guid bookId)
        {
            return await _set
                .Include(r => r.Events)
                .Where(r => r.BookId == bookId)
                .OrderByDescending(r => r.Events.Max(e => e.Date))
                .FirstOrDefaultAsync();
        }

        public async Task<Reservation> GetReservationWithInfo(Guid id)
        {
            return await _set
                .Include(r => r.Book)
                .ThenInclude(b => b.BookInfo)
                .Include(User => User.Member)
                    .ThenInclude(User => User.User)
                .Include(User => User.Employee)
                    .ThenInclude(User => User.User)
                .Include(r => r.Events)
                .Include(r => r.Book)
                    .ThenInclude(Book => Book.BookInfo)
                        .ThenInclude(BookInfo => BookInfo.Title)
                .Include(r => r.Book)
                    .ThenInclude(Book => Book.BookInfo)
                        .ThenInclude(BookInfo => BookInfo.Author)
                .Include(r => r.Book)
                    .ThenInclude(Book => Book.BookInfo)
                        .ThenInclude(BookInfo => BookInfo.Publisher)
                .Include(r => r.Book)
                    .ThenInclude(Book => Book.BookInfo)
                        .ThenInclude(BookInfo => BookInfo.Picture)
                .Include(r => r.Book)
                    .ThenInclude(Book => Book.BookInfo)
                        .ThenInclude(BookInfo => BookInfo.BookGenre)
                            .ThenInclude(BookGenre => BookGenre.Genre)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Reservation>> GetReservationsWithInfo()
        {
            return await _set
                .Include(r => r.Book)
                .ThenInclude(b => b.BookInfo)
                .Include(User => User.Member)
                    .ThenInclude(User => User.User)
                .Include(User => User.Employee)
                    .ThenInclude(User => User.User)
                .Include(r => r.Events)
                .Include(r => r.Book)
                    .ThenInclude(Book => Book.BookInfo)
                        .ThenInclude(BookInfo => BookInfo.Title)
                .Include(r => r.Book)
                    .ThenInclude(Book => Book.BookInfo)
                        .ThenInclude(BookInfo => BookInfo.Author)
                .Include(r => r.Book)
                    .ThenInclude(Book => Book.BookInfo)
                        .ThenInclude(BookInfo => BookInfo.Publisher)
                .Include(r => r.Book)
                    .ThenInclude(Book => Book.BookInfo)
                        .ThenInclude(BookInfo => BookInfo.Picture)
                .Include(r => r.Book)
                    .ThenInclude(Book => Book.BookInfo)
                        .ThenInclude(BookInfo => BookInfo.BookGenre)
                            .ThenInclude(BookGenre => BookGenre.Genre)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetAllActiveReservations()
        {
            var activeEventTypes = new List<Domain.Enums.EventType> { Domain.Enums.EventType.BookReserved, Domain.Enums.EventType.BookBorrowed, Domain.Enums.EventType.BookRenewed };
            return await _set
                .Include(r => r.Events)
                .ThenInclude(e => e.EventType)
                .Where(r => r.Events.Any(e => activeEventTypes.Contains(e.EventType)))
                .ToListAsync();
        }

    }
}
