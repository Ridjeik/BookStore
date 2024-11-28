using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class BookInfo
    {
        public Guid Id { get; set; }
        public DateTime DatePublication { get; set; }
        public int? PageNumber { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double Rating { get; set; }
        public Guid TitleId { get; set;}
        public Guid AuthorId { get; set; }
        public Guid PublisherId { get; set; }
        public Guid PictureId { get; set; }
        public Picture Picture { get; set; }
        public Publisher Publisher { get; set; }
        public Author Author { get; set; }
        public TitleBook Title { get; set; }
        public ICollection<BookGenre> BookGenre { get; set; } = new HashSet<BookGenre>();
        public ICollection<Book> Books { get; set; } = new HashSet<Book>();
    }
}
