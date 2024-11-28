using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Shelf
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set;}
        public Room Room { get; set;}
        public ICollection<Row> Rows { get; set; } = new HashSet<Row>();
        public ICollection<ShelfGenre> ShelfGenre { get; set;} = new HashSet<ShelfGenre>();
    }
}
