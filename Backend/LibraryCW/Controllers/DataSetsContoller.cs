using BLL.Models;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryCW.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataSetsContoller : ControllerBase
    {
        private readonly IDataSetsService _dataSetsService;
        public DataSetsContoller(IDataSetsService dataSetsService)
        {
            _dataSetsService = dataSetsService;
        }

        [HttpGet("CreateDataSet")]
        public async Task<IActionResult> CreateDataSet()
        {
            var result = await _dataSetsService.CreateDataSet();
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("DeleteDataSet")]
        public async Task<IActionResult> DeleteDataSet()
        {
            var result = await _dataSetsService.ClearDatabaseAsync();
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
