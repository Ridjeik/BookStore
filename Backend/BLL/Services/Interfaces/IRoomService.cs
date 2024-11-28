using BLL.Models.Responses;
using DAL.Repositories.Implemantations;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IRoomService
    {
        Task<Result<IEnumerable<Room>>> GetRooms();
        public Task<Result<Room>> GetRoomByIdAsync(Guid id);
        public Task<Result> UpdateRoomAsync(Guid id, string name);
        Task<Result> CreateRoomAsync(string name);
        Task<Result> DeleteRoomAsync(Guid id);
    }
}
