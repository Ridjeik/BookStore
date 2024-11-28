using AutoMapper;
using BLL.Helpers;
using BLL.Models;
using BLL.Models.Responses;
using BLL.Services.Interfaces;
using DAL.Repositories.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Implemantation
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IPublisherRepository _publisherRepository;
        private readonly IBookInfoRepository _bookInfoRepository;
        private readonly ITitleBookRepository _titleRepository;
        private readonly IMapper _mapper;
        public BookService(IBookRepository bookRepository,
            IMapper mapper,
            IGenreRepository genreRepository,
            IAuthorRepository authorRepository,
            IPublisherRepository publisherRepository,
            IBookInfoRepository bookInfoRepository,
            ITitleBookRepository titleRepository)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            _genreRepository = genreRepository;
            _authorRepository = authorRepository;
            _publisherRepository = publisherRepository;
            _bookInfoRepository = bookInfoRepository;
            _titleRepository = titleRepository;
        }

        public async Task<Result<IEnumerable<BookInfo>>> GetBooks()
        {
            var books = await _bookInfoRepository.GetBooksWithInfo();
            return Result<IEnumerable<BookInfo>>.Success(books);
        }

        public async Task<Result<IEnumerable<Book>>> GetBookExamplesById(Guid id)
        {
            var bookInfos = (await _bookInfoRepository.GetBookWithInfoAndBooks(id));
            if (bookInfos == null)
            {
                return Result<IEnumerable<Book>>.Failure("Book not found");
            }
            var books = bookInfos.Books.Where(a => a.Available);
            return Result<IEnumerable<Book>>.Success(books);
        }

        public async Task<Result<BookInfo>> GetBookById(Guid id)
        {
            var book = await _bookInfoRepository.GetBookWithInfoAndBooks(id);
            if (book == null)
            {
                return Result<BookInfo>.Failure("Book not found");
            }
            return Result<BookInfo>.Success(book);
        }

        public async Task<Result<Book>> GetExampleBookById(Guid id)
        {
            var book = await _bookRepository.GetBookWithInfo(id);
            if (book == null)
            {
                return Result<Book>.Failure("Book not found");
            }
            return Result<Book>.Success(book);
        }

        public async Task<Result<IEnumerable<BookInfo>>> GetLastBooks()
        {
            var books = await _bookInfoRepository.GetBooksWithInfoQueryable()
                .Include(bi => bi.Books)
                .Where(bi => bi.Books.Any())
                .OrderByDescending(b => b.Id)
                .Take(3)
                .ToListAsync();

            return Result<IEnumerable<BookInfo>>.Success(books);
        }

        public async Task<Result<IEnumerable<BookInfo>>> GetRecomendedBooks(int top)
        {
            var books = await _bookInfoRepository.GetBooksWithInfoQueryable()
                .Include(bi => bi.Books)
                .Where(bi => bi.Books.Any())
                .OrderByDescending(b => b.Rating)
                .Take(top)
                .ToListAsync();

            return Result<IEnumerable<BookInfo>>.Success(books);
        }
        public async Task<Result<IEnumerable<BookInfo>>> GetMostPopularBooks(int top)
        {
            try
            {
                return Result<IEnumerable<BookInfo>>.Success(await _bookInfoRepository.GetMostPopularBooks(top));
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<BookInfo>>.Failure(ex.Message);
            }
        }



        public async Task<Result> CreateBook(CreateBookDto createBookDto)
        {
            try
            {
                if (createBookDto.DatePublication > DateTime.Now)
                {
                    return Result.Failure("Publication date cannot be in the future");
                }

                var genreNames = createBookDto.GenreNames;
                var foundGenres = (await _genreRepository.FindAsync(g => genreNames.Contains(g.Name))).ToList();
                var foundGenreNames = foundGenres.Select(g => g.Name).ToList();

                var missingGenreNames = genreNames.Except(foundGenreNames).ToList();

                foreach (var missingGenreName in missingGenreNames)
                {
                    var newGenre = new Genre { Name = missingGenreName };
                    await _genreRepository.CreateAsync(newGenre);
                    foundGenres.Add(newGenre);
                }

                var author = (await _authorRepository.FindAsync(a => a.Name == createBookDto.AuthorName)).FirstOrDefault();
                if (author == null)
                {
                    author = new Author { Name = createBookDto.AuthorName };
                    await _authorRepository.CreateAsync(author);
                }

                var publisher = (await _publisherRepository.FindAsync(a => a.Name == createBookDto.PublisherName)).FirstOrDefault();
                if (publisher == null)
                {
                    publisher = new Publisher { Name = createBookDto.PublisherName };
                    await _publisherRepository.CreateAsync(publisher);
                }

                var title = (await _titleRepository.FindAsync(a => a.Name == createBookDto.TitleName)).FirstOrDefault();
                if (title == null)
                {
                    title = new TitleBook { Name = createBookDto.TitleName };
                    await _titleRepository.CreateAsync(title);
                }

                var bookInfo = new BookInfo
                {
                    Author = author,
                    Publisher = publisher,
                    BookGenre = foundGenres.Select(g => new BookGenre { Genre = g }).ToList(),
                    Title = title,
                    DatePublication = createBookDto.DatePublication,
                    Description = createBookDto.Description,
                    Picture = new Picture { Url = createBookDto.PictureUrl },
                    Rating = createBookDto.Rating,
                    Price = createBookDto.Price,
                    PageNumber = createBookDto.PageNumber
                };

                await _bookInfoRepository.CreateAsync(bookInfo);

                var book = new Book
                {
                    Available = true,
                    BookInfo = bookInfo,
                    NumberInRow = createBookDto.NumberInRow,
                    RowId = createBookDto.RowId
                };

                await _bookRepository.CreateAsync(book);

                return Result.Success();
            }
            catch(Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }




        public async Task<PagedResponse<BookInfo>> GetBooksBySearch(string searchString, string genreString, string authorString, string sortOrder, PaginationFilter filter)
        {
            var query = _bookInfoRepository.GetBooksWithInfoQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(b => b.Title.Name.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(genreString))
            {
                query = query.Where(b => b.BookGenre.Any(g => g.Genre.Name.Contains(genreString)));
            }

            if (!string.IsNullOrEmpty(authorString))
            {
                query = query.Where(b => b.Author.Name.Contains(authorString));
            }

            switch (sortOrder)
            {
                case "DateAddedAsc":
                    query = query.OrderBy(b => b.Id);
                    break;
                case "DateAddedDesc":
                    query = query.OrderByDescending(b => b.Id);
                    break;
                case "DatePublicationAsc":
                    query = query.OrderBy(b => b.DatePublication);
                    break;
                case "DatePublicationDesc":
                    query = query.OrderByDescending(b => b.DatePublication);
                    break;
            }

            var totalRecords = await query.CountAsync();
            var data = await query.Skip((filter.PageNumber - 1) * filter.PageSize)
                                  .Take(filter.PageSize)
                                  .ToListAsync();

            var pagedResponse = new PagedResponse<BookInfo>
            {
                Data = data,
                TotalDataCount = totalRecords,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)filter.PageSize)
            };

            return pagedResponse;
        }


    }
}
