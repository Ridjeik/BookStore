using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Picture
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public ICollection<BookInfo> BookInfos { get; set; } = new HashSet<BookInfo>();

    }
}
