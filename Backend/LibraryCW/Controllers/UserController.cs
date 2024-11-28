using BLL.Services.Interfaces;
using LibraryCW.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryCW.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IReservationService _reservationService;

        public UserController(IUserService userService, IReservationService reservationService)
        {
            _userService = userService;
            _reservationService = reservationService;
        }

        public async Task<IActionResult> Profile()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            if (User.IsInRole("Member"))
            {
                var user = await _userService.GetMemberById(userId);
                var reservations = await _reservationService.GetReservationsByMemberId(userId);
                var model = new UserProfileViewModel
                {
                    Member = user,
                    Reservations = reservations.Data,
                };
                return View(model);
            }
            else if (User.IsInRole("Employee"))
            {
                var user = await _userService.GetEmployeeById(userId);
                var reservations = await _reservationService.GetReservationByEmployeeId(userId);
                var model = new UserProfileViewModel
                {
                    Employee = user,
                    Reservations = reservations.Data,
                };
                return View(model);
            }
            return View();
        }
    }

}
