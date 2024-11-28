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
    public class RoomRepository : BaseRepository<Room, Guid>, IRoomRepository
    {
        public RoomRepository(DbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Room>> GetAllRoomsWithInfoAsync()
        {
            return await _set
                .Include(r => r.Shelfs)
                    .ThenInclude(s => s.ShelfGenre)
                        .ThenInclude(r => r.Genre)
                                .ToListAsync();
        }
    }
}
