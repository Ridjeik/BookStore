using DAL.Repositories.Implementation;
using DAL.Repositories.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Implemantations
{
    public class ShelfRepository : BaseRepository<Shelf, Guid>, IShelfRepository
    {
        public ShelfRepository(DbContext context) : base(context)
        {

        }

        public async Task<Shelf?> GetShelfById(Guid shelfId)
        {
            return await _set
                .Include(s => s.ShelfGenre)
                .ThenInclude(sg => sg.Genre)
                .Include(s => s.Rows)
                .ThenInclude(r => r.Books).ThenInclude(b => b.BookInfo.Title)
                .Include(s => s.Rows)
                .ThenInclude(r => r.Books).ThenInclude(b => b.BookInfo.Picture)
                .FirstOrDefaultAsync(s => s.Id == shelfId);
        }

        public async Task<IEnumerable<Shelf>> GetShelvesForRoom(Guid roomId)
        {
            return await _set
                .Where(s => s.RoomId == roomId)
                .Include(s => s.ShelfGenre)
                .ThenInclude(sg => sg.Genre)
                .ToListAsync();
        }
    }
}
