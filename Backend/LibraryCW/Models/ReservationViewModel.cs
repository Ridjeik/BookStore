using BLL.Models;
using Domain.Entities;
using Domain.Enums;

namespace LibraryCW.Models
{
    public class ReservationViewModel
    {
        public BookInfoViewModel BookInfoViewModel { get; set; }
        public string MemberUsername { get; set; }
        public string ReservationDate { get; set; }
        public string EmployeeUsername { get; set; }
        public string BookId { get; set; }
        public string MemberId { get; set; }
        public string EmployeeId { get; set; }
    }


    public class ReservationDetailsViewModel
    {
        public string ReservationId { get; set; }
        public BookInfoViewModel BookInfoViewModel { get; set; }
        public Member Member { get; set; }
        public string ReservationDate { get; set; }
        public Employee Employee { get; set; }
        public string BookId { get; set; }
        public List<ReservationEventViewModel> Events { get; set; }
    }

    public class ReservationEventViewModel
    {
        public EventType EventType { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }

}
