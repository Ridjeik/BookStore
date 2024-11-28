using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Row
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public Guid ShelfId { get; set; }
        public Shelf Shelf { get; set; }
        public ICollection<Book> Books { get; set; } = new HashSet<Book>();
    }

}
