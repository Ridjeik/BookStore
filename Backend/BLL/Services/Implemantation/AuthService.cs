using AutoMapper;
using Azure.Core;
using BLL.Constants;
using BLL.Models;
using BLL.Models.Responses;
using BLL.Services.Interfaces;
using DAL.Repositories.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BLL.Services.Implemantation
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService tokenService;
        private readonly IMemberRepository _memberRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public AuthService(UserManager<User> userManager,
            ITokenService tokenService,
            IMemberRepository memberRepository,
            IEmployeeRepository employeeRepository,
            IMapper mapper)
        {
            _userManager = userManager;
            _memberRepository = memberRepository;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            this.tokenService = tokenService;
        }

        public async Task<Result<UserDetailsDto>> RegisterMember(RegisterDto userDto, decimal balance)
        {
            var user = new User
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                Name = userDto.Name,
                Surname = userDto.Surname,
                PhoneNumber = userDto.PhoneNumber
            };

            var email = await _userManager.FindByEmailAsync(user.Email);
            if (email != null)
            {
                return Result<UserDetailsDto>.Failure(Messages.Auth.EmailInUse);
            }

            var result = await _userManager.CreateAsync(user, userDto.Password);
            await _userManager.AddToRoleAsync(user, "Member");

            if (result.Succeeded)
            {   
                var member = new Member { UserId = user.Id, Balance = balance };
                await _memberRepository.CreateAsync(member);

                var map = _mapper.Map<UserDetailsDto>(user);
                return Result<UserDetailsDto>.Success(map);
            }
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));

            return Result<UserDetailsDto>.Failure(errors);
        }

        public async Task<Result<UserDetailsDto>> RegisterEmployee(RegisterDto userDto, decimal salary)
        {
            var user = new User
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                Name = userDto.Name,
                Surname = userDto.Surname,
                PhoneNumber = userDto.PhoneNumber
            };

            var email = await _userManager.FindByEmailAsync(user.Email);
            if (email != null)
            {
                return Result<UserDetailsDto>.Failure(Messages.Auth.EmailInUse);
            }

            var result = await _userManager.CreateAsync(user, userDto.Password);
            await _userManager.AddToRoleAsync(user, "Employee");

            if (result.Succeeded)
            {
                var employee = new Employee { UserId = user.Id };
                employee.Salary = salary;
                await _employeeRepository.CreateAsync(employee);

                var map = _mapper.Map<UserDetailsDto>(user);
                return Result<UserDetailsDto>.Success(map);
            }
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result<UserDetailsDto>.Failure(errors);
        }




        public async Task<Result<AuthSuccessResponse>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user is null)
            {
                return Result<AuthSuccessResponse>.Failure("User not found");
            }

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!result)
            {
                return Result<AuthSuccessResponse>.Failure(Messages.Auth.PasswordError);
            }
            var role = await _userManager.GetRolesAsync(user);
            var map = _mapper.Map<UserDetailsDto>(user);
            return Result<AuthSuccessResponse>.Success(await tokenService.GenerateToken(map, role.FirstOrDefault() ?? ""));
        }

        /*public async Task<Result> Logout(string refreshToken)
        {
            // If we don't have refresh token it means that user is already logged out
            if (refreshToken is null)
            {
                return Result.Success();
            }

            var response = (await tokenService.FindRefreshToken(refreshToken));
            var refToken = response.Data.SingleOrDefault();

            if (refToken is null)
            {
                return Result.Failure(Messages.Auth.RefreshTokenNotFound);
            }
            refToken.Invalidated = true;

            await tokenService.UpdateRefreshToken(refToken);

            return Result.Success();
        }*/

    }
}
