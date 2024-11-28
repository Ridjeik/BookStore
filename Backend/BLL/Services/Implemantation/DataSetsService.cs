using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Models.Responses;
using BLL.Services.Interfaces;
using CsvHelper;
using DAL.Repositories.Interfaces;
using Domain.Entities;
using CsvHelper.Configuration.Attributes;
using Microsoft.EntityFrameworkCore;
using DAL.EF;
using BLL.Models;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace BLL.Services.Implemantation
{

    public class BookRecord
    {
        [Name("bookId")]
        public string Id { get; set; }

        [Name("title")]
        public string Title { get; set; }

        [Name("series")]
        public string Series { get; set; }

        [Name("author")]
        public string Author { get; set; }

        [Name("rating")]
        public double Rating { get; set; }

        [Name("description")]
        public string Description { get; set; }

        [Name("language")]
        public string Language { get; set; }

        [Name("isbn")]
        public string Isbn { get; set; }

        [Name("genres")]
        public string Genres { get; set; }

        [Name("characters")]
        public string Characters { get; set; }

        [Name("bookFormat")]
        public string BookFormat { get; set; }

        [Name("edition")]
        public string Edition { get; set; }

        [Name("pages")]
        public int? Pages { get; set; }

        [Name("publisher")]
        public string Publisher { get; set; }

        [Name("publishDate")]
        public string PublishDate { get; set; }

        [Name("firstPublishDate")]
        public string FirstPublishDate { get; set; }

        [Name("awards")]
        public string Awards { get; set; }

        [Name("numRatings")]
        public int NumRatings { get; set; }

        [Name("ratingsByStars")]
        public string RatingsByStars { get; set; }

        [Name("likedPercent")]
        public double LikedPercent { get; set; }

        [Name("setting")]
        public string Setting { get; set; }

        [Name("coverImg")]
        public string CoverImg { get; set; }

        [Name("bbeScore")]
        public double BbeScore { get; set; }

        [Name("bbeVotes")]
        public int BbeVotes { get; set; }

        [Name("price")]
        public double? Price { get; set; }
    }

    public class UserRecord
    {
        [Name("id")]
        public int Id { get; set; }

        [Name("Name")]
        public string Name { get; set; }

        [Name("Surname")]
        public string Surname { get; set; }

        [Name("Email")]
        public string Email { get; set; }

        [Name("PhoneNumber")]
        public string PhoneNumber { get; set; }

        [Name("Password")]
        public string Password { get; set; }

        [Name("Balance")]
        public decimal Balance { get; set; }

        [Name("UserName")]
        public string UserName { get; set; }

        [Name("Salary")]
        public decimal Salary { get; set; }
    }



    public class DataSetsService : IDataSetsService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IPublisherRepository _publisherRepository;
        private readonly ITitleBookRepository _titleBookRepository;
        private readonly IPictureRepository _pictureRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IBookGenreRepository _bookGenreRepository;
        private readonly IRowRepository _rowRepository;
        private readonly IShelfRepository _shelfRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IShelfGenreRepository _shelfGenreRepository;
        private readonly IBookInfoRepository _bookInfoRepository;
        private readonly IAuthService _authService;
        private readonly IMemberRepository _memberRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly LibraryDbContext _context;


        public DataSetsService(IBookRepository bookRepository,
            IAuthorRepository authorRepository,
            IPublisherRepository publisherRepository,
            ITitleBookRepository titleBookRepository,
            IPictureRepository pictureRepository,
            IGenreRepository genreRepository,
            IBookGenreRepository bookGenreRepository,
            IRowRepository rowRepository,
            IShelfRepository shelfRepository,
            IRoomRepository roomRepository,
            IShelfGenreRepository shelfGenreRepository,
            IAuthService authService,
            LibraryDbContext context,
            IBookInfoRepository bookInfoRepository,
            IMemberRepository memberRepository,
            IEmployeeRepository employeeRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _publisherRepository = publisherRepository;
            _titleBookRepository = titleBookRepository;
            _pictureRepository = pictureRepository;
            _genreRepository = genreRepository;
            _bookGenreRepository = bookGenreRepository;
            _rowRepository = rowRepository;
            _shelfRepository = shelfRepository;
            _roomRepository = roomRepository;
            _shelfGenreRepository = shelfGenreRepository;
            _authService = authService;
            _context = context;
            _bookInfoRepository = bookInfoRepository;
            _memberRepository = memberRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<Result> ClearDatabaseAsync()
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync("EXEC ClearDatabase");
            }
            catch (Exception ex)
            {
                Result.Failure(ex.Message);
            }
            return Result.Success();
        }

        private string CleanDescription(string description)
        {
            if (description == null)
            {
                return "";
            }

            string cleanedDescription = Regex.Replace(description, @"[^\u0000-\u007F]+", string.Empty);

            return cleanedDescription.Length > 1000 ? cleanedDescription.Substring(0, 1000) : cleanedDescription;
        }
        public async Task<Result> CreateDataSet()
        {
            try
            {
                using var reader = new StreamReader("C:\\Users\\Admin\\Downloads\\books_1.Best_Books_Ever.csv");
                using var readerUsers = new StreamReader("C:\\Users\\Admin\\Downloads\\USERS.csv");
                var config = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    BadDataFound = null,
                    ShouldSkipRecord = context =>
                    {
                        if (context.Row.ColumnCount > 0 && context.Row[0].StartsWith("BadData"))
                        {
                            Console.WriteLine($"Bad data found: {context.Row[0]}");
                            return true;
                        }
                        return false;
                    }
                };

                using var csv = new CsvReader(reader, config);
                using var csv2 = new CsvReader(readerUsers, config);


                // Map the data to your entities
                var records = csv.GetRecords<BookRecord>();
                var users = csv2.GetRecords<UserRecord>();

                foreach (var record in users)
                {
                    var user = new RegisterDto
                    {
                        Name = record.Name,
                        Surname = record.Surname,
                        Email = record.Email,
                        PhoneNumber = record.PhoneNumber,
                        UserName = record.UserName,
                        Password = record.Password,
                    };
                    Random random = new Random();
                    int chance = random.Next(1, 101); // Generates a random number between 1 and 100

                    if (chance <= 90)
                    {
                        var result = await _authService.RegisterMember(user, record.Balance);
                    }
                    else
                    {
                        var result = await _authService.RegisterEmployee(user, record.Salary);
                    }
                }

                var members = await _memberRepository.GetAllAsQueryable().Include(m => m.User).ToListAsync();
                var employees = await _employeeRepository.GetAllAsQueryable().Include(m => m.User).ToListAsync();

                int i = 0;
                int bookCount = 0;
                Row row = null;
                Shelf shelf = null; // Declare shelf here
                Room room = null;
                foreach (var record in records)
                {
                    // Create a new room
                    if(i % 200 == 0)
                    {
                        room = new Room
                        {
                            Name = $"Room {Guid.NewGuid()}", // Generate a random room name
                        };
                        await _roomRepository.CreateAsync(room);
                    }
                    if (i % 20 == 0)
                    {
                        shelf = new Shelf
                        {
                            RoomId = room.Id,
                        };
                        await _shelfRepository.CreateAsync(shelf);
                        bookCount = 0;
                    }

                    // Check if the author already exists, if not create a new one
                    var author = (await _authorRepository.FindAsync(a => a.Name.ToLower() == record.Author.ToLower())).FirstOrDefault();
                    if (author == null)
                    {
                        author = new Author
                        {
                            Name = record.Author.Length > 100 ? record.Description.Substring(0, 100) : record.Author,
                        };
                        await _authorRepository.CreateAsync(author);
                    }

                    // Check if the publisher already exists, if not create a new one
                    var publisher = (await _publisherRepository.FindAsync(p => p.Name.ToLower() == record.Publisher.ToLower())).FirstOrDefault();
                    if (publisher == null)
                    {
                        publisher = new Publisher
                        {
                            Name = record.Publisher,
                        };
                    await _publisherRepository.CreateAsync(publisher);
                    }

                    // Check if the picture already exists, if not create a new one
                    var picture = (await _pictureRepository.FindAsync(p => p.Url.ToLower() == record.CoverImg.ToLower())).FirstOrDefault();
                    if (picture == null)
                    {
                        picture = new Picture
                        {
                            Url = record.CoverImg,
                        };
                        await _pictureRepository.CreateAsync(picture);
                    }

                    // Check if the title already exists, if not create a new one
                    var title = (await _titleBookRepository.FindAsync(t => t.Name.ToLower() == record.Title.ToLower())).FirstOrDefault();
                    if (title == null)
                    {
                        title = new TitleBook
                        {
                            Name = record.Title,
                        };
                        await _titleBookRepository.CreateAsync(title);
                    }


                    List<Genre> genres = new List<Genre>();
                    var genreNames = JsonConvert.DeserializeObject<List<string>>(record.Genres) ?? new List<string>();
                    foreach (var genreName in genreNames)
                    {
                        var genre = (await _genreRepository.FindAsync(g => g.Name == genreName)).FirstOrDefault();
                        if (genre == null)
                        {
                            genre = new Genre
                            {
                                Name = genreName,
                            };
                            await _genreRepository.CreateAsync(genre);
                        }
                        genres.Add(genre);
                    }

                    // Create a new book
                    Random random = new Random();
                    var bookInfo = new BookInfo
                    {
                        Description = CleanDescription(record.Description),
                        PageNumber = record.Pages ?? 0,
                        TitleId = title.Id,
                        Rating = record.Rating,
                        Price = record.Price ?? random.Next(1, 20),
                        AuthorId = author.Id,
                        PublisherId = publisher.Id,
                        PictureId = picture.Id,
                    };
                    DateTime publishDate;
                    string[] formats = { "M/d/yyyy", "MMMM dd yyyy", "yyyy", "MMMM yyyy", "M/dd/yy", "MMMM d'st' yyyy", "MMMM d'nd' yyyy", "MMMM d'rd' yyyy", "MMMM d'th' yyyy" };
                    if (DateTime.TryParseExact(record.PublishDate.Trim(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out publishDate))
                    {
                        bookInfo.DatePublication = publishDate;
                    }
                    else
                    {
                        Console.WriteLine($"Invalid PublishDate: {record.PublishDate}");
                    }
                    await _bookInfoRepository.CreateAsync(bookInfo);

                    int bookCountForThisRecord = random.Next(1, 21); // Generate a random number of books between 1 and 20

                    for (int j = 0; j < bookCountForThisRecord; j++)
                    {
                        if (bookCount % 10 == 0)
                        {
                            row = new Row
                            {
                                Number = bookCount / 10, // This will give row numbers 0, 1, 2, etc.
                                ShelfId = shelf.Id,
                            };
                            await _rowRepository.CreateAsync(row);
                        }

                        var book = new Book
                        {
                            BookInfoId = bookInfo.Id,
                            Available = true,
                            NumberInRow = (j % 20) + 1, // Use the current number in row
                            RowId = row.Id,
                        };

                        await _bookRepository.CreateAsync(book);
                        bookCount++;

                        Random random2 = new Random();
                        int reservationChance = random2.Next(1, 101); // Generates a random number between 1 and 100

                        if (reservationChance <= 20) // 20% chance
                        { 

                            if (members.Any() && employees.Any())
                            {
                                var randomMember = members[random.Next(members.Count)];
                                var randomEmployee = employees[random.Next(employees.Count)];

                                var reservation = new Reservation
                                {
                                    BookId = book.Id,
                                    MemberId = randomMember.UserId.ToString(),
                                    EmployeeId = randomEmployee.UserId.ToString(),
                                    Description = $"Reservation for {randomMember.User.Name} made by {randomEmployee.User.Name}"
                                };

                                _context.Reservations.Add(reservation);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }

                    // Create a new genre
                    int u = 0;
                    foreach (var genreName in genreNames)
                    {
                        var bookGenre = new BookGenre
                        {
                            BookInfoId = bookInfo.Id,
                            GenreId = genres.ElementAt(u).Id,
                        };
                        await _bookGenreRepository.CreateAsync(bookGenre);
                        u++;
                    }

                    if (i == 1000) break;
                    i++;
                    bookCount++;
                }
                return Result.Success();
            }
            catch (DbUpdateException ex)
            {
                return Result.Failure(ex.InnerException.Message);
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace);
            }
        }
    }
}
