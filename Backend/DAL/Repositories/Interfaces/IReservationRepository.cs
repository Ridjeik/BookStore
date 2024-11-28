using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IReservationRepository : IBaseRepository<Reservation, Guid>
    {
        public Task<IEnumerable<Reservation>> GetReservationsByMemberId(string userId);
        public Task<IEnumerable<Reservation>> GetReservationsByEmployeeId(string userId);
        public Task<Reservation> GetLatestReservationByBookId(Guid bookId);
        public Task<Reservation> GetReservationWithInfo(Guid id);
        public Task<IEnumerable<Reservation>> GetReservationsWithInfo();
    }
}
