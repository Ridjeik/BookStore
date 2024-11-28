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
    public class EmployeeRepository : BaseRepository<Employee, string>, IEmployeeRepository
    {
        public EmployeeRepository(DbContext context) : base(context)
        {

        }

        public async Task<Employee> GetEmployeeWithInfoAsync(string id)
        {
            return await _set.Include(x => x.User).FirstOrDefaultAsync(x => x.UserId == id);
        }

        public async Task<IEnumerable<Employee>> GetEmployeeWithInfoAsync()
        {
            return await _set.Include(x => x.User).ToListAsync();
        }
    }
}
