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
    public class MemberRepository : BaseRepository<Member, string>, IMemberRepository
    {
        public MemberRepository(DbContext context) : base(context)
        {

        }

        public async Task<Member> GetMemberWithInfoAsync(string id)
        {
            return await _set.Include(x => x.User).FirstOrDefaultAsync(x => x.UserId == id);
        }

        public async Task<IEnumerable<Member>> GetMembersWithInfoAsync()
        {
            return await _set.Include(x => x.User).ToListAsync();
        }

        public async Task<Member?> GetMemberWithInfoAndReservationsAsync(string id)
        {
            return await _set.Include(x => x.User).Include(x => x.Reservations).FirstOrDefaultAsync(x => x.UserId == id);
        }

        public async Task<IEnumerable<Member>> GetMembersWithInfoAndReservationsAsync()
        {
            return await _set.Include(x => x.User).Include(x => x.Reservations).ToListAsync();
        }

        public async Task<Member?> GetMemberByNameAsync(string name)
        {
            return await _set.Include(x => x.User).FirstOrDefaultAsync(x => x.User.UserName == name);
        }
    }
}
