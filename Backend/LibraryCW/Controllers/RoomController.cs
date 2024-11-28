using BLL.Services.Implemantation;
using BLL.Services.Interfaces;
using LibraryCW.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryCW.Controllers
{
    public class RoomController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly IShelfService _shelfService;
        private readonly IGenreService _genreService;
        private readonly IRowService _rowService;
        public RoomController(IRoomService roomService, IShelfService shelfService, IGenreService genreService, IRowService rowService)
        {
            _roomService = roomService;
            _shelfService = shelfService;
            _genreService = genreService;
            _rowService = rowService;
        }

        [HttpGet]
        [Authorize(Policy = "AdminOrEmployee")]
        public async Task<IActionResult> DisplayRooms()
        {
            // Get the list of rooms from your database
            var rooms = await _roomService.GetRooms(); // Replace with your actual code to get rooms
            if(rooms == null)
            {
                return NotFound();
            }
            // Convert the rooms to the display model
            var model = rooms.Data.Select(r => new RoomDisplayModel
            {
                RoomId = r.Id.ToString(),
                RoomName = r.Name,
                Shelves = r.Shelfs.Select(shelf => new ShelfDisplayModel
                {
                    ShelfId = shelf.Id.ToString(),
                    ShelfGenres = shelf.ShelfGenre.Select(s => s.Genre.Name).ToList(),
                }).ToList()
            }).ToList();

            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "AdminOrEmployee")]
        public async Task<IActionResult> ShelfRows(Guid shelfId)
        {
            // Retrieve the shelf data from your data source using shelfId
            var result = await _shelfService.GetShelfById(shelfId);
            if (!result.IsSuccess)
            {
                return NotFound();
            }
            var shelf = result.Data;

            // Map the shelf data to a view model
            var shelfViewModel = new ShelfDisplayModel
            {
                ShelfId = shelf.Id.ToString(),
                ShelfGenres = shelf.ShelfGenre.Select(g => g.Genre.Name).ToList(),
                Rows = shelf.Rows.Select(r => new RowDisplayModel
                {
                    RowId = r.Id.ToString(),
                    RowNumber = r.Number,
                    Books = r.Books.Select(b => new BookDisplayModel
                    {
                        Id = b.Id.ToString(),
                        Title = b.BookInfo.Title.Name,
                        Picture = b.BookInfo.Picture.Url,
                        BookNumberInRow = b.NumberInRow
                    }).ToList()
                }).ToList()
            };

            return View(shelfViewModel);
        }

        [HttpGet]
        public IActionResult RoomCreate()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> RoomUpdate(Guid roomId)
        {
            var room = await _roomService.GetRoomByIdAsync(roomId);
            if (room == null)
            {
                return NotFound();
            }
            var roomData = room.Data;

            var model = new RoomUpdateModel
            {
                Id = roomData.Id,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RoomUpdate(RoomUpdateModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _roomService.UpdateRoomAsync(model.Id, model.Name);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
                return Json(new { success = false });
            }

            return Json(new { success = false });
        }


        public async Task<IActionResult> DeleteRoom(Guid roomId)
         {
             await _roomService.DeleteRoomAsync(roomId);
             return RedirectToAction("DisplayRooms");
         }

        [HttpPost]
        public async Task<IActionResult> ShelfCreate(ShelfCreateModel model)
        {
            if (ModelState.IsValid)
            {
                await _shelfService.CreateShelf(model.RoomId, model.Genres);
                return RedirectToAction("DisplayRooms");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RowCreate(Guid shelfId, int number)
        {
            var result = await _rowService.CreateRowAsync(shelfId, number);
            if (result.IsSuccess)
            {
                return RedirectToAction("ShelfRows", new { shelfId });
            }
            return NotFound();
        }
        public async Task<IActionResult> RowDelete(Guid rowId)
        {
            await _rowService.DeleteRowAsync(rowId);
            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> ShelfUpdate(Guid shelfId)
        {
            var result = await _shelfService.GetShelfById(shelfId);
            if (!result.IsSuccess)
            {
                return NotFound();
            }
            var model= new ShelfUpdateModel { ShelfId = shelfId, Genres = result.Data.ShelfGenre.Select(g => g.GenreId).ToList() };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ShelfUpdate(ShelfUpdateModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _shelfService.UpdateShelf(model.ShelfId, model.Genres);
                if (result.IsSuccess)
                {
                    return RedirectToAction("DisplayRooms");
                }
                ModelState.AddModelError("", "There was an error updating the shelf.");
            }

            return View(model);
        }
        public async Task<IActionResult> ShelfDelete(Guid shelfId)
        {
            await _shelfService.DeleteShelf(shelfId);
            return RedirectToAction("DisplayRooms");
        }


        [HttpGet]
        public async Task<IActionResult> ShelfCreate()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetGenres()
        {
            var genres = await _genreService.GetGenres(); // Replace with your actual code to get genres

            return Json(genres.Data);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOrEmployee")]
        public async Task<IActionResult> RoomCreate(string name)
        {
            if (ModelState.IsValid)
            {
                var result = await _roomService.CreateRoomAsync(name);
                if (result.IsSuccess)
                {
                    return RedirectToAction("DisplayRooms");
                }
                ModelState.AddModelError("", "There was an error creating the room.");
            }

            return View();
        }


    }
}
