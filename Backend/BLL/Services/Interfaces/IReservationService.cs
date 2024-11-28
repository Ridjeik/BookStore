using BLL.Models.Responses;
using BLL.Models;
using DAL.Repositories.Implemantations;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IReservationService
    {
        Task<Result<Reservation>> GetReservationById(Guid id);
        public Task<Result<IEnumerable<Reservation>>> GetReservations();
        public Task<Result<IEnumerable<Reservation>>> GetReservationsByMemberId(string memberId);
        public Task<Result<Reservation>> GetLatestReservationByBookId(Guid bookId);
        public Task<Result<Reservation>> CreateReservation(ReservationDto reservationDto);
        public Task<Result<Reservation>> UpdateReservation(UpdateReservationDto updateReservationDto);
        public Task<Result<IEnumerable<Reservation>>> GetReservationByEmployeeId(string employeeId);
    }
}
