using BLL.Models;
using BLL.Models.Responses;
using BLL.Services.Interfaces;
using DAL.Repositories.Implemantations;
using DAL.Repositories.Interfaces;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Implemantation
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository reservationRepository;
        private readonly IEventRepository eventRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public ReservationService(IReservationRepository reservationRepository, IEventRepository eventRepository, IEmployeeRepository employeeRepository)
        {
            this.reservationRepository = reservationRepository;
            this.eventRepository = eventRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<Result<Reservation>> GetReservationById(Guid id)
        {
            var reservation = await reservationRepository.GetReservationWithInfo(id);
            if (reservation == null)
            {
                return Result<Reservation>.Failure("Reservation not found");
            }
            return Result<Reservation>.Success(reservation);
        }

        public async Task<Result<IEnumerable<Reservation>>> GetReservations()
        {
            var reservations = await reservationRepository.GetReservationsWithInfo();
            return Result<IEnumerable<Reservation>>.Success(reservations);
        }

        public async Task<Result<IEnumerable<Reservation>>> GetReservationsByMemberId(string memberId)
        {
            var reservations = await reservationRepository.GetReservationsByMemberId(memberId);
            return Result<IEnumerable<Reservation>>.Success(reservations);
        }

        public async Task<Result<IEnumerable<Reservation>>> GetReservationByEmployeeId(string employeeId)
        {
            var reservations = await reservationRepository.GetReservationsByEmployeeId(employeeId);
            return Result<IEnumerable<Reservation>>.Success(reservations);
        }

        public async Task<Result<Reservation>> GetLatestReservationByBookId(Guid bookId)
        {
            var reservation = await reservationRepository.GetLatestReservationByBookId(bookId);
            return Result<Reservation>.Success(reservation);
        }

        public async Task<Result<Reservation>> CreateReservation(ReservationDto reservationDto)
        {
            var reservation = new Reservation
            {
                BookId = reservationDto.BookId,
                MemberId = reservationDto.MemberId,
                Description = reservationDto.Description,
                EmployeeId = reservationDto.EmployeeId,
            };
            await reservationRepository.CreateAsync(reservation);

            await eventRepository.CreateAsync(
                new Event()
                {
                    Date = DateTime.Now,
                    EventType = Domain.Enums.EventType.BookReserved,
                    ReservationId = reservation.Id
                }
            );

            return Result<Reservation>.Success(reservation);
        }

        public async Task<Result<Reservation>> UpdateReservation(UpdateReservationDto updateReservationDto)
        {
            var reservation = await reservationRepository.GetReservationWithInfo(updateReservationDto.Id);
            if (reservation == null)
            {
                return Result<Reservation>.Failure("Reservation not found");
            }

            // Update the Description if it's not null
            if (updateReservationDto.Description != null)
            {
                reservation.Description = updateReservationDto.Description;
            }

            // Update the EventType if it's not null
            if (updateReservationDto.EventType != null)
            {
                var eventToUpdate = reservation.Events.FirstOrDefault(e => e.EventType == updateReservationDto.EventType);
                if(eventToUpdate == null || eventToUpdate.EventType < updateReservationDto.EventType)
                {
                    var newEvent = new Event
                    {
                        Date = DateTime.Now,
                        EventType = updateReservationDto.EventType,
                        ReservationId = reservation.Id
                    };
                    reservation.Events.Add(newEvent);
                }
            }

            // Update the Employee if UsernameEmployee is not null
            if (updateReservationDto.UsernameEmployee != null)
            {
                var employee = (await _employeeRepository.GetEmployeeWithInfoAsync()).Where(a => a.User.UserName == updateReservationDto.UsernameEmployee).FirstOrDefault();
                if (employee != null)
                {
                    reservation.EmployeeId = employee.UserId;
                }
            }

            await reservationRepository.UpdateAsync(reservation);
            return Result<Reservation>.Success(reservation);
        }
    }
}
