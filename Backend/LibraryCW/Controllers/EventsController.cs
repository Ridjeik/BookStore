using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc;
using Domain.Enums;
using System;
using System.Threading.Tasks;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

public class EventController : Controller
{
    private readonly IEventService _eventService; // Replace with your actual service

    public EventController(IEventService eventService) // Replace with your actual service
    {
        _eventService = eventService;
    }

    [HttpPost]
    [Authorize(Policy = "AdminOrEmployee")]
    public async Task<IActionResult> CreateEvent(Guid reservationId, EventType eventType)
    {
        try
        {
            await _eventService.CreateEvent(reservationId, eventType); // Replace with your actual service method

            TempData["SuccessMessage"] = "Event created successfully.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "Error creating event: " + ex.Message;
        }

        return RedirectToAction("ReservationDetails", "Reservation", new { reservationId = reservationId });
    }
}

