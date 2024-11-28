﻿using DAL.Repositories.Implementation;
using DAL.Repositories.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Implemantations
{
    public class BookGenreRepository : BaseRepository<BookGenre, Guid>, IBookGenreRepository
    {
        public BookGenreRepository(DbContext context) : base(context)
        {

        }
    }
}
