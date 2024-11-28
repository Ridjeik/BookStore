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
    public interface IShelfService
    {
        public Task<Result<IEnumerable<Shelf>>> GetShelvesByRoom(Guid roomId);
        public Task<Result<Shelf>> GetShelfById(Guid shelfId);
        Task<Result> CreateShelf(Guid roomId, List<Guid> genres);
        Task<Result> DeleteShelf(Guid shelfId);
        Task<Result> UpdateShelf(Guid shelfId, List<Guid> genres);
    }
}
