﻿using BLL.Models.Responses;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IGenreService
    {
        Task<Result<IEnumerable<Genre>>> GetGenres();
    }
}