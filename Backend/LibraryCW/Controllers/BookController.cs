using BLL.Models;
using BLL.Models.Responses;
using BLL.Services.Implemantation;
using BLL.Services.Interfaces;
using CsvHelper;
using Domain.Entities;
using LibraryCW.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Xml.Serialization;

namespace LibraryCW.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IMemoryCache _cache;
        private readonly IGenreService _genreService;
        private readonly ITitleService _titleService;
        private readonly IPublisherService _publisherService;
        private readonly IAuthorService _authorService;

        public BookController(IBookService bookService, IMemoryCache cache, IGenreService genreService, ITitleService titleService, IPublisherService publisherService, IAuthorService authorService)
        {
            _bookService = bookService;
            _cache = cache;
            _genreService = genreService;
            _titleService = titleService;
            _publisherService = publisherService;
            _authorService = authorService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new BookViewModel();
            ViewData["Book"] = "Book";
            model.MostPopularBooks = await GetOrSetCacheAsync("MostPopularBooks", () => _bookService.GetMostPopularBooks(3));
            model.RecommendedBooks = await GetOrSetCacheAsync("RecommendedBooks", () => _bookService.GetRecomendedBooks(3));
            model.RecentlyAddedBooks = await GetOrSetCacheAsync("LastBooks", () => _bookService.GetLastBooks());
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> BooksInfo(Guid id)
        {
            var result = await _bookService.GetBookById(id);

            if (!result.IsSuccess)
            {
                return NotFound();
            }

            var model = new BookInfoViewModel
            {
                BookInfo = result.Data,
                Books = result.Data.Books.Where(b => b.Available)
            };
            return View(model);
        }

        private async Task<IEnumerable<BookInfo>> GetOrSetCacheAsync(string cacheKey, Func<Task<Result<IEnumerable<BookInfo>>>> retrieveData)
        {
            if (!_cache.TryGetValue(cacheKey, out IEnumerable<BookInfo> cachedData))
            {
                var result = await retrieveData();
                cachedData = result.Data;
                _cache.Set(cacheKey, cachedData);
            }

            return cachedData;
        }
        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var result = await _bookService.GetBooks();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetTitles(string q)
        {
            var titles = await _titleService.GetAllTitles();
            var filteredTitles = titles.Data.Where(t => t.Name.Contains(q));
            return Json(filteredTitles);
        }

        [HttpGet]
        public async Task<IActionResult> GetAuthors(string q)
        {
            var authors = await _authorService.GetAllAuthors();
            var filteredAuthors = authors.Data.Where(a => a.Name.Contains(q));
            return Json(filteredAuthors);
        }

        [HttpGet]
        public async Task<IActionResult> GetPublishers(string q)
        {
            var publishers = await _publisherService.GetAllPublishers();
            var filteredPublishers = publishers.Data.Where(p => p.Name.Contains(q));
            return Json(filteredPublishers);
        }



        [HttpGet]
        public IActionResult BookCreate(Guid rowId, int numberInRow)
        {
            ViewData["RowId"] = rowId;
            ViewData["NumberInRow"] = numberInRow;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BookCreate(BookCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var createBookDto = new CreateBookDto
            {
                RowId = model.RowId,
                TitleName = model.TitleName,
                AuthorName = model.AuthorName,
                PublisherName = model.PublisherName,
                DatePublication = model.DatePublication,
                Description = model.Description,
                Price = model.Price,
                Rating = model.Rating,
                PictureUrl = model.PictureUrl,
                PageNumber = model.PageNumber,
                GenreNames = model.GenreNames,
                NumberInRow = model.NumberInRow,
            };

            var result = await _bookService.CreateBook(createBookDto);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Error);
                return View(model);
            }

            return RedirectToAction("AllBooks", "Book");
        }

        [HttpGet]
        public async Task<IActionResult> BookView(Guid bookId)
        {
            var bookResult = await _bookService.GetExampleBookById(bookId);
            if (!bookResult.IsSuccess)
            {
                return NotFound();
            }
            // Create the BookDetailsViewModel
            var model = new BookDetailsViewModel
            {
                BookId = bookId.ToString(),
                BookInfo = bookResult.Data.BookInfo,
                Available = bookResult.Data.Available,
                NumberInRow = bookResult.Data.NumberInRow,
                RowId = bookResult.Data.RowId,
                Reservations = bookResult.Data.Reservations
            };

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> GetGenres(string q)
        {
            var result = await _genreService.GetGenres();
            var genres = result.Data.Where(g => g.Name.Contains(q));
            return Json(genres);
        }



        [HttpGet]
        public async Task<IActionResult> AllBooks(string searchString, string categoryString, string authorString, string sortOrder, int pageNumber = 1, int pageSize = 12)
        {
            var books = await _bookService.GetBooksBySearch(searchString, categoryString, authorString, sortOrder, new PaginationFilter(pageNumber, pageSize));
            return View(books);
        }

        [HttpGet]
        [Authorize(Policy = "AdminOrEmployee")]
        public async Task<IActionResult> ExportAllBooks(string format, string searchString = null, string genreString = null, string authorString = null, string sortOrder = null)
        {
            var books = await _bookService.GetBooksBySearch(searchString, genreString, authorString, sortOrder, new PaginationFilter(1, int.MaxValue));

            if (books == null)
            {
                return NotFound();
            }

            var exportBooks = books.Data.Select(b => new ExportBookInfo
            {
                Id = b.Id,
                Title = b.Title.Name,
                Author = b.Author.Name,
                Genres = string.Join(", ", b.BookGenre.Select(g => g.Genre.Name)),
                DatePublication = b.DatePublication.ToString("yyyy-MM-dd"),
                Description = b.Description,
                Price = b.Price,
                Rating = b.Rating,
                PictureUrl = b.Picture.Url,
                PageNumber = b.PageNumber ?? 0,
                Count = b.Books.Count

            }).ToList();

            if (format == "csv")
            {
                var builder = new StringBuilder();
                var csvWriter = new StringWriter(builder);
                using (var csv = new CsvWriter(csvWriter, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(exportBooks);
                }
                return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "books.csv");
            }
            else if (format == "json")
            {
                var json = JsonSerializer.Serialize(exportBooks);
                return File(Encoding.UTF8.GetBytes(json), "application/json", "books.json");
            }
            else if (format == "xml")
            {
                var xmlSerializer = new XmlSerializer(typeof(List<ExportBookInfo>));
                var stringWriter = new StringWriter();
                xmlSerializer.Serialize(stringWriter, exportBooks);
                return File(Encoding.UTF8.GetBytes(stringWriter.ToString()), "application/xml", "books.xml");
            }

            return BadRequest("Invalid format");
        }

        public async Task<IActionResult> Statistics()
        {
            var popularBooks = await _bookService.GetMostPopularBooks(10);
            var model = new List<PopularBook>();
            foreach (var book in popularBooks.Data)
            {
                var borrowedCount = book.Books.Count(b => !b.Available);
                var availableCount = book.Books.Count(b => b.Available);
                model.Add(new PopularBook
                {
                    BookInfoId = book.Id.ToString(),
                    Title = book.Title.Name,
                    ReservationCount = book.Books.Sum(a => a.Reservations.Count),
                    BorrowedCount = borrowedCount,
                    AvailableCount = availableCount
                });
                model.OrderByDescending(a => a.ReservationCount);
            }

            return View(model);
        }


    }
    public class ExportBookInfo
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genres { get; set; }
        public string DatePublication { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double Rating { get; set; }
        public string PictureUrl { get; set; }
        public int PageNumber { get; set;}
        public int Count { get; set; }
    }
}
