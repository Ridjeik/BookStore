using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Employee
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public decimal? Salary { get; set; }
        public ICollection<Reservation> ReservationCreator { get; set; } = new HashSet<Reservation>();
    }

}
