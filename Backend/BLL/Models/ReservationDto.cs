using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class ReservationDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public Guid BookId { get; set; }
        public string MemberId { get; set; }
        public string EmployeeId { get; set; }
    }

    public class CreateReservationDto
    {
        public Guid BookId { get; set; }
        public string UsernameMember { get; set; }
    }

    public class UpdateReservationDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string UsernameEmployee { get; set; }
        public Domain.Enums.EventType EventType { get; set; }
    }
}
