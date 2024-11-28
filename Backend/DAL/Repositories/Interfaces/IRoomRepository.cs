﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IRoomRepository : IBaseRepository<Room, Guid>
    {
        Task<IEnumerable<Room>> GetAllRoomsWithInfoAsync();
    }
}
