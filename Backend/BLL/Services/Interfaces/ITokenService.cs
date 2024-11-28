using BLL.Models;
using BLL.Models.Responses;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface ITokenService
    {
        public Task<AuthSuccessResponse> GenerateToken(UserDetailsDto user, string role);
    }
}
