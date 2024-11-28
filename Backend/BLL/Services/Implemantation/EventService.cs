using BLL.Models.Responses;
using BLL.Services.Interfaces;
using DAL.Repositories.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Implemantation
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IReservationRepository _reservationRepository;

        public EventService(IEventRepository eventRepository, IReservationRepository reservationRepository)
        {
            _eventRepository = eventRepository;
            _reservationRepository = reservationRepository;
        }

        public async Task<Result> CreateEvent(Guid reservationId, EventType eventType)
        {
            var reservation = await _reservationRepository.GetReservationWithInfo(reservationId);
            if (reservation == null)
            {
                return Result.Failure("Reservation not found");
            }
            if(reservation.Events.LastOrDefault()?.EventType >= eventType)
            {
                return Result.Failure("Event can`t be added");
            }
            var @event = new Domain.Entities.Event
            {
                Reservation = reservation,
                EventType = eventType,
                Date = DateTime.Now
            };

            await _eventRepository.CreateAsync(@event);
            return Result.Success();
        }
    }
}
