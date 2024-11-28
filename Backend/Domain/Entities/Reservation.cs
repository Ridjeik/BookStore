using Domain.Entities;

namespace Domain.Entities
{
    public class Reservation
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public Guid BookId { get; set; }
        public Book Book { get; set; }
        public string MemberId { get; set; }
        public Member Member { get; set; }
        public string EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public ICollection<Event> Events { get; set; } = new HashSet<Event>();
    }
}
