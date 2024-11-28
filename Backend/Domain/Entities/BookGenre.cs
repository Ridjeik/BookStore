using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class BookGenre
    {
        public Guid BookInfoId { get; set; }
        public BookInfo BookInfo { get; set; }
        public Guid GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
