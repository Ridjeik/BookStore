using BLL.Models.Responses;
using BLL.Services.Interfaces;
using DAL.Repositories.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Implemantation
{
    public class ShelfService : IShelfService
    {
        private readonly IShelfRepository shelfRepository;

        public ShelfService(IShelfRepository shelfRepository)
        {
            this.shelfRepository = shelfRepository;
        }

        public async Task<Result> UpdateShelf(Guid shelfId, List<Guid> genres)
        {
            var shelf = await shelfRepository.GetShelfById(shelfId);
            if (shelf == null)
            {
                return Result.Failure("Shelf not found");
            }
            shelf.ShelfGenre = genres.Select(g => new ShelfGenre { GenreId = g }).ToList();
            await shelfRepository.UpdateAsync(shelf);
            return Result.Success();
        }

        public async Task<Result> DeleteShelf(Guid shelfId)
        {
            var shelf = await shelfRepository.GetShelfById(shelfId);
            if (shelf == null)
            {
                return Result.Failure("Shelf not found");
            }
            foreach(var genre in shelf.ShelfGenre)
            {
                shelf.ShelfGenre.Remove(genre);
            }
            await shelfRepository.DeleteAsync(shelfId);
            return Result.Success();
        }

        public async Task<Result<IEnumerable<Shelf>>> GetShelvesByRoom(Guid roomId)
        {
            var shelves = await shelfRepository.GetShelvesForRoom(roomId);
            return Result<IEnumerable<Shelf>>.Success(shelves);
        }

        public async Task<Result<Shelf>> GetShelfById(Guid shelfId)
        {
            var shelf = await shelfRepository.GetShelfById(shelfId);
            if (shelf == null)
            {
                return Result<Shelf>.Failure("Shelf not found");
            }
            return Result<Shelf>.Success(shelf);
        }

        public async Task<Result> CreateShelf(Guid roomId, List<Guid> genres)
        {

            var shelf = new Shelf
            {
                RoomId = roomId,
                ShelfGenre = genres.Select(g => new ShelfGenre { GenreId = g }).ToList()
            };

            await shelfRepository.CreateAsync(shelf);

            return Result.Success();
        }


    }
}
