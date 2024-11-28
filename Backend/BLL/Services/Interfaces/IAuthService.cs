using BLL.Models;
using BLL.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Result<AuthSuccessResponse>> Login(LoginDto loginDto);
        public Task<Result<UserDetailsDto>> RegisterEmployee(RegisterDto userDto, decimal salary);
        public Task<Result<UserDetailsDto>> RegisterMember(RegisterDto userDto, decimal balance);

        //Task<Result> Logout(string refreshToken);
    }
}
