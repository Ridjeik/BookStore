using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IMemberRepository : IBaseRepository<Member, string>
    {
        public Task<Member> GetMemberWithInfoAsync(string id);
        public Task<IEnumerable<Member>> GetMembersWithInfoAsync();
        public Task<Member?> GetMemberWithInfoAndReservationsAsync(string id);
        public Task<IEnumerable<Member>> GetMembersWithInfoAndReservationsAsync();
        public Task<Member?> GetMemberByNameAsync(string name);
    }
}
