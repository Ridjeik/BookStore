using BLL.Models;
using BLL.Models.Responses;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IUserService
    {
        public Task<MemberDetailsDto> GetMemberById(string memberId);
        public Task DeleteMember(string memberId);
        public Task<EmployeeDto> GetEmployeeById(string userId);
        public Task DeleteEmployee(string employeeId);
        public Task<IEnumerable<EmployeeDto>> GetEmployees();
        public Task<IEnumerable<MemberDetailsDto>> GetMembers();
        Task<Result<Member>> GetMemberByUsername(string userName);
    }
}