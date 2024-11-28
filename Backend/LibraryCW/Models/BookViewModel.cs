using BLL.Models;
using Domain.Entities;

namespace LibraryCW.Models
{
    public class BookViewModel
    {
        public IEnumerable<BookInfo> MostPopularBooks { get; set; } = new List<BookInfo>();
        public IEnumerable<BookInfo> RecommendedBooks { get; set; } = new List<BookInfo>();
        public IEnumerable<BookInfo> RecentlyAddedBooks { get; set; } = new List<BookInfo>();
    }

    public class BookInfoViewModel
    {
        public BookInfo BookInfo { get; set; }
        public IEnumerable<Book> Books { get; set; } = new List<Book>();
    }

    public class BookCreateViewModel
    {
        public Guid RowId { get; set; }
        public int NumberInRow { get; set; }
        public DateTime DatePublication { get; set; }
        public int PageNumber { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double Rating { get; set; }
        public string PictureUrl { get; set; }
        public string TitleName { get; set; }
        public string AuthorName { get; set; }
        public string PublisherName { get; set; }
        public List<string> GenreNames { get; set; }
    }

    public class BookDetailsViewModel
    {
        public string BookId { get; set; }
        public BookInfo BookInfo { get; set; }
        public bool Available { get; set; }
        public int NumberInRow { get; set; }
        public Guid RowId { get; set; }
        public IEnumerable<Reservation> Reservations { get; set; }
    }

    public class PopularBook
    {
        public string BookInfoId { get; set; }
        public int ReservationCount { get; set; }
        public int BorrowedCount { get; set; }
        public int AvailableCount { get; set; }
        public string Title { get; set; }
    }
}
