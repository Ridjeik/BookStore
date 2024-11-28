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

    public class RowService : IRowService
    {
        private readonly IRowRepository rowRepository;

        public RowService(IRowRepository rowRepository)
        {
            this.rowRepository = rowRepository;
        }
        public async Task<Result> CreateRowAsync(Guid shelfId, int number)
        {
            var row = new Row
            {
                ShelfId = shelfId,
                Number = number
            };

            await rowRepository.CreateAsync(row);

            return Result.Success();
        }

        public async Task<Result> DeleteRowAsync(Guid id)
        {
            var row = await rowRepository.GetAsync(id);
            if (row != null)
            {
                await rowRepository.DeleteAsync(id);
                return Result.Success();
            }
            else
            {
                return Result.Failure("Row not found");
            }
        }

        public async Task<Result<Row>> GetRowByIdAsync(Guid id)
        {
            var row = await rowRepository.GetAsync(id);
            if (row == null)
            {
                return Result<Row>.Failure("Row not found");
            }
            return Result<Row>.Success(row);
        }

        public async Task<Result> UpdateRowAsync(Guid id, Guid shelfId)
        {
            var row = await rowRepository.GetAsync(id);
            if (row == null)
            {
                return Result.Failure("Row not found");
            }
            row.ShelfId = shelfId;
            await rowRepository.UpdateAsync(row);
            return Result.Success();
        }
    }
}
