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
    public class TitleService : ITitleService
    {
        private readonly ITitleBookRepository _titleRepository;

        public TitleService(ITitleBookRepository titleRepository)
        {
            _titleRepository = titleRepository;
        }

        public async Task<Result<IEnumerable<TitleBook>>> GetAllTitles()
        {
            return Result<IEnumerable<TitleBook>>.Success(await _titleRepository.GetAllAsync());
        }
    }
}
