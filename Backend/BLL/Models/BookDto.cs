using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class BookDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int YearPublication { get; set; }
        public string Publisher { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public bool Available { get; set; }
        public List<string> Genres { get; set; }
        public int PageNumber { get; set; }
    }

    public class CreateBookDto
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
}
