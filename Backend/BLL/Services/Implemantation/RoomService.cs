using BLL.Models.Responses;
using BLL.Services.Interfaces;
using DAL.Repositories.Implemantations;
using DAL.Repositories.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Implemantation
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository roomRepository;

        public RoomService(IRoomRepository roomRepository)
        {
            this.roomRepository = roomRepository;
        }
        public async Task<Result> CreateRoomAsync(string name)
        {
            var room = new Room
            {
                Name = name
            };

            await roomRepository.CreateAsync(room);

            return Result.Success();
        }

        public async Task<Result> DeleteRoomAsync(Guid id)
        {
            var room = await roomRepository.GetAsync(id);
            if (room != null)
            {
                await roomRepository.DeleteAsync(id);
                return Result.Success();
            }
            else
            {
                return Result.Failure("Room not found");
            }
        }
        public async Task<Result<IEnumerable<Room>>> GetRooms()
        {
            var rooms = await roomRepository.GetAllRoomsWithInfoAsync();
            return Result<IEnumerable<Room>>.Success(rooms);
        }

        public async Task<Result<Room>> GetRoomByIdAsync(Guid id)
        {
            var room = await roomRepository.GetAsync(id);
            if (room == null)
            {
                return Result<Room>.Failure("Room not found");
            }
            return Result<Room>.Success(room);
        }

        public async Task<Result> UpdateRoomAsync(Guid id, string name)
        {
            var room = await roomRepository.GetAsync(id);
            if (room != null)
            {
                room.Name = name;

                await roomRepository.UpdateAsync(room);
                return Result.Success();
            }
            else
            {
                return Result.Failure("Room not found");
            }
        }
    }
}
