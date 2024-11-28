using BLL.Models;
using BLL.Models.Responses;
using BLL.Services.Interfaces;
using DAL.Repositories.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Implemantation
{
    public class UserService : IUserService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public UserService(IMemberRepository memberRepository, IEmployeeRepository employeeRepository)
        {
            _memberRepository = memberRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<MemberDetailsDto> GetMemberById(string memberId)
        {
            var member = await _memberRepository.GetMemberWithInfoAsync(memberId);
            return new MemberDetailsDto
            {
                UserName = member.User.UserName,
                Email = member.User.Email,
                PhoneNumber = member.User.PhoneNumber,
                Balance = member.Balance,
            };
        }

        public async Task<IEnumerable<MemberDetailsDto>> GetMembers()
        {
            var members = await _memberRepository.GetAllAsync();
            return members.Select(m => new MemberDetailsDto
            {
                UserName = m.User.UserName,
                Email = m.User.Email,
                PhoneNumber = m.User.PhoneNumber,
                Balance = m.Balance,
            });
        }


        public async Task<Result<Member>> GetMemberByUsername(string userName)
        {
            var member = await _memberRepository.GetMemberByNameAsync(userName);
            if(member == null)
            {
                return Result<Member>.Failure("Member not found");
            }
            return Result<Member>.Success(member);
        }
        public async Task DeleteMember(string memberId)
        {
            await _memberRepository.DeleteAsync(memberId);
        }

        // Employee methods
        public async Task<EmployeeDto> GetEmployeeById(string userId)
        {
            var employee = await _employeeRepository.GetEmployeeWithInfoAsync(userId);
            return new EmployeeDto
            {
                UserName = employee.User.UserName,
                Email = employee.User.Email,
                PhoneNumber = employee.User.PhoneNumber,
                Salary = employee.Salary ?? 0,
            };
        }

        public async Task<IEnumerable<EmployeeDto>> GetEmployees()
        {
            var employees = await _employeeRepository.GetEmployeeWithInfoAsync();
            return employees.Select(e => new EmployeeDto
            {
                Id = e.UserId,
                UserName = e.User.UserName,
                Email = e.User.Email,
                PhoneNumber = e.User.PhoneNumber,
                Salary = e.Salary ?? 0,
            });
        }

        public async Task DeleteEmployee(string employeeId)
        {
            await _employeeRepository.DeleteAsync(employeeId);
        }
    }

}
