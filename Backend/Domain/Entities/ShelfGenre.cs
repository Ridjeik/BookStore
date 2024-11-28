using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ShelfGenre
    {
        public Guid ShelfId { get; set; }
        public Shelf Shelf { get; set; }
        public Guid GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
