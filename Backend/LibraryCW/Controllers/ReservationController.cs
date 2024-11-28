using BLL.Models;
using BLL.Services.Interfaces;
using DAL.Repositories.Interfaces;
using Domain.Entities;
using Domain.Enums;
using LibraryCW.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LibraryCW.Controllers
{
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly IUserService _userService;
        private readonly IBookService _bookService;

        public ReservationController(IReservationService reservationService, IUserService userService, IBookService bookService)
        {
            _reservationService = reservationService;
            _userService = userService;
            _bookService = bookService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> MakeReservationMember(CreateReservationDto createReservationDto)
        {
            var employees = await _userService.GetEmployees();
            var count = employees.Count();
            if (count == 0)
            {
                return BadRequest("No employees found");
            }
            var random = new Random();
            var randomEmployee = employees.ElementAt(random.Next(count));

            string idMember = null;
            string usernameClaimMember = null;
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
            var usernameClaim = User.Claims.FirstOrDefault(c => c.Type == "UserName");
            idMember = idClaim?.Value;
            usernameClaimMember = usernameClaim?.Value;

            if(idMember == null)
            {
                return BadRequest("No member found");
            }
            var reservationDto = new ReservationDto
            {
                BookId = createReservationDto.BookId,
                EmployeeId = randomEmployee.Id,
                MemberId = idMember,
                Description = "Reservation was made " + DateTime.Now.ToString() + " by member " + usernameClaimMember,
            };

            var book = await _bookService.GetBookExamplesById(createReservationDto.BookId);
            var bookInfo = book.Data.FirstOrDefault().BookInfo;
            if(!book.IsSuccess)
            {
                return BadRequest(book.Error);
            }

            var model = new ReservationViewModel
            {
                BookInfoViewModel = new BookInfoViewModel
                {
                    BookInfo = bookInfo,
                },
                BookId = createReservationDto.BookId.ToString(),
                ReservationDate = DateTime.Now.ToShortDateString(),
                MemberUsername = usernameClaimMember,
                EmployeeUsername = randomEmployee.UserName,
                EmployeeId = randomEmployee.Id,
                MemberId = idMember,
            };

            return View(model);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ReservationDetails(Guid reservationId)
        {
            // Fetch the reservation details from your service or repository
            var result = await _reservationService.GetReservationById(reservationId);

            if (!result.IsSuccess)
            {
                return NotFound();
            }
            var reservation = result.Data;

            var reservationDetailsViewModel = new ReservationDetailsViewModel
            {
                ReservationId = reservation.Id.ToString(),
                BookInfoViewModel = new BookInfoViewModel
                {
                    BookInfo = reservation.Book.BookInfo
                },
                Member = reservation.Member,
                Employee = reservation.Employee,
                BookId = reservation.BookId.ToString(),
                Events = reservation.Events.Select(e => new ReservationEventViewModel
                {
                    EventType = e.EventType,
                    Date = e.Date,
                    Description = GetEventDescription(e.EventType)
                }).ToList()
            };

            return View(reservationDetailsViewModel);
        }

        private string GetEventDescription(EventType eventType)
        {
            switch (eventType)
            {
                case EventType.BookReserved:
                    return "The book was reserved.";
                case EventType.BookBorrowed:
                    return "The book was borrowed.";
                case EventType.BookRenewed:
                    return "The book loan was renewed.";
                case EventType.BookRemoved:
                    return "The book was removed from the reservation.";
                case EventType.BookReturned:
                    return "The book was returned.";
                default:
                    return "Unknown event type.";
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateReservation(string bookInfoId, string employeeId, string memberUsername)
        {
            string idMember = null;
            string usernameClaimMember = memberUsername;
            if(User.IsInRole("Employee") || User.IsInRole("Admin"))
            {
                var member = await _userService.GetMemberByUsername(memberUsername);
                if(!member.IsSuccess)
                {
                    return Json(new { success = false, error = member.Error });
                }
                idMember = member.Data.UserId;
            }
            else
            {
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
                var usernameClaim = User.Claims.FirstOrDefault(c => c.Type == "UserName");
                idMember = idClaim?.Value;
                usernameClaimMember = usernameClaim?.Value;
            }

            var book = await _bookService.GetBookExamplesById(Guid.Parse(bookInfoId));
            if (!book.IsSuccess)
            {
                return Json(new { success = false, error = book.Error });
            }
            if(!book.Data.Any())
            {
                return Json(new { success = false, error = "No book found" });
            }
            if (idMember == null)
            {
                return Json(new { success = false, error = "No member found" });
            }
            var reservationDto = new ReservationDto
            {
                BookId = book.Data.FirstOrDefault().Id,
                EmployeeId = employeeId,
                MemberId = idMember,
                Description = "Reservation was made " + DateTime.Now.ToString() + " by member " + usernameClaimMember,
            };

            var result = await _reservationService.CreateReservation(reservationDto);

            if (!result.IsSuccess)
            {
                return Json(new { success = false, error = result.Error });
            }

            return Json(new { success = true, reservationId = result.Data.Id });
        }

        [HttpPost]
        [Authorize(Policy = "AdminOrEmployee")]
        public async Task<IActionResult> MakeReservationEmployee(Guid bookInfoId)
        {
            var book = await _bookService.GetBookById(bookInfoId);
            var employeeId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            if (!book.IsSuccess || !book.Data.Books.Any(a => a.Available))
            {
                return View("MakeReservationEmployee", new ReservationViewModel { EmployeeId = employeeId });
            }

            // Redirect to the reservation details page
            return View("MakeReservationEmployee", new ReservationViewModel { BookInfoViewModel = new BookInfoViewModel { BookInfo = book.Data }, EmployeeId = employeeId });
        }

    }
}
