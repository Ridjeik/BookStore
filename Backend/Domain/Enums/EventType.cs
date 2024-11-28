using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum EventType
    {
        BookReserved = 1,
        BookBorrowed = 2,
        BookRenewed = 3,
        BookRemoved = 4,
        BookReturned = 5,
    }
}
