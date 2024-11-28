using BLL.Models.Responses;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IRowService
    {
        Task<Result> CreateRowAsync(Guid shelfId, int number);
        Task<Result> DeleteRowAsync(Guid id);
        Task<Result<Row>> GetRowByIdAsync(Guid id);
        Task<Result> UpdateRowAsync(Guid id, Guid shelfId);
    }
}
