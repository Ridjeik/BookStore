using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IShelfRepository : IBaseRepository<Shelf, Guid>
    {
        public Task<Shelf?> GetShelfById(Guid shelfId);
        public Task<IEnumerable<Shelf>> GetShelvesForRoom(Guid roomId);
    }
}
