using BLL.Models;
using Domain.Entities;

namespace LibraryCW.Models
{
    public class UserProfileViewModel
    {
        public MemberDetailsDto Member { get; set; }
        public EmployeeDto Employee { get; set; }
        public IEnumerable<Reservation> Reservations { get; set; }
    }
}

