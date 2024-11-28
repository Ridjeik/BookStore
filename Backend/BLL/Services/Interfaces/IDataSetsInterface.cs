﻿using BLL.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IDataSetsService
    {
        public Task<Result> CreateDataSet();
        Task<Result> ClearDatabaseAsync();
    }
}
