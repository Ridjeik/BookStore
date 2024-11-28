using BLL.Models;
using BLL.Models.Responses;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IBookService
    {
        Task<Result<IEnumerable<BookInfo>>> GetBooks();
        Task<Result<IEnumerable<BookInfo>>> GetLastBooks();
        Task<Result<IEnumerable<BookInfo>>> GetRecomendedBooks(int top);
        Task<Result<IEnumerable<BookInfo>>> GetMostPopularBooks(int top);
        Task<Result> CreateBook(CreateBookDto createBookDto);
        Task<Result<BookInfo>> GetBookById(Guid id);
        Task<Result<Book>> GetExampleBookById(Guid id);
        Task<Result<IEnumerable<Book>>> GetBookExamplesById(Guid id);
        Task<PagedResponse<BookInfo>> GetBooksBySearch(string searchString, string genreString, string authorString, string sortOrder, PaginationFilter filter);
    }
}
