﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Author
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<BookInfo> BookInfos { get; set; } = new HashSet<BookInfo>();
    }
}
