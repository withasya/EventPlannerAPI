using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using EventPlannerAPI.Models;
using EventPlannerAPI.Data;

namespace EventPlannerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EventController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/event   Etkinlik oluşturulur
        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] Event newEvent)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            // Organizer'ı ayarla
            newEvent.OrganizerId = userId;
            newEvent.Status = EventStatus.Pending;

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Etkinlik başarıyla oluşturuldu. Admin onayı bekleniyor.",
                eventId = newEvent.Id
            });
        }




        // GET: api/event   Sadece onaylanmış etkinlkler listelenir
        [HttpGet]
        public async Task<IActionResult> GetApprovedEvents()
        {
            var approvedEvents = await _context.Events
                .Where(e => e.Status == EventStatus.Approved)
                .Include(e => e.Organizer)
                .ToListAsync();

            return Ok(approvedEvents);
        }



        // Sadece Addminler

        // GET: api/event/pending Bekleyen etkinlikler
        [HttpGet("pending")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPendingEvents()
        {
            var pendingEvents = await _context.Events
                .Where(e => e.Status == EventStatus.Pending)
                .Include(e => e.Organizer)
                .ToListAsync();

            return Ok(pendingEvents);
        }



        // PUT: api/event/{id}/approve    Etkinliği Onayla
        [HttpPut("{id}/approve")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveEvent(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
                return NotFound();

            @event.Status = EventStatus.Approved;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Etkinlik onaylandı." });
        }





        // PUT: api/event/{id}/reject      Etkinliği Reddet
        [HttpPut("{id}/reject")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RejectEvent(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
                return NotFound();

            @event.Status = EventStatus.Rejected;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Etkinlik reddedildi." });
        }



    }
}
