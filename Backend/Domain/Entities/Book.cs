using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Book
    {
        public Guid Id { get; set; }
        public bool Available { get; set; }
        public int NumberInRow { get; set; }
        public Guid RowId { get; set; }
        public Row Row { get; set; }
        public Guid BookInfoId { get; set; }
        public BookInfo BookInfo { get; set; }
        public ICollection<Reservation> Reservations { get; set; } = new HashSet<Reservation>();
    }

}
