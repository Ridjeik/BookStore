using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Event
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public Enums.EventType EventType { get; set; }
        public Guid ReservationId { get; set; }
        public Reservation Reservation { get; set; }
    }
}
