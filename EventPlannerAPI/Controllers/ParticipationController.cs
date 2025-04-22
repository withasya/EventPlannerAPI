using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using EventPlannerAPI.Models;
using EventPlannerAPI.Data;
using EventPlannerAPI.Dtos;

namespace EventPlannerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ParticipationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ParticipationController(ApplicationDbContext context)
        {
            _context = context;
        }


        //Kullanıcı etkinliğe katılıyor
        // POST: api/participation?eventId=1
        [HttpPost]
        public async Task<IActionResult> JoinEvent([FromQuery] int eventId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized("Kullanıcı bulunamadı.");

            // Etkinlik var mı ve onaylanmış mı?
            var @event = await _context.Events
                .FirstOrDefaultAsync(e => e.Id == eventId && e.Status == EventStatus.Approved);

            if (@event == null)
                return BadRequest("Etkinlik bulunamadı ya da henüz onaylanmamış.");

            // Kullanıcı zaten bu etkinliğe katılmış mı?
            var existingParticipation = await _context.UserEvents
                .FirstOrDefaultAsync(ue => ue.UserId == userId && ue.EventId == eventId);

            if (existingParticipation != null)
                return BadRequest("Bu etkinliğe zaten katıldınız.");

            // Katılımı oluştur
            var userEvent = new UserEvent
            {
                UserId = userId,
                EventId = eventId
            };

            _context.UserEvents.Add(userEvent);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Etkinliğe başarıyla katıldınız." });
        }




        // Kullanıcının katıldığı etkinlikleri getirme
        // GET: api/participation/user
        [HttpGet("user")]
        public async Task<IActionResult> GetUserParticipations()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized("Kullanıcı bulunamadı.");

            var participations = await _context.UserEvents
                .Where(ue => ue.UserId == userId)
                .Include(ue => ue.Event)
                .ThenInclude(e => e.Organizer)
                .Select(ue => new ParticipationDTO
                {
                    EventId = ue.EventId,
                    EventTitle = ue.Event.Title,
                    EventLocation = ue.Event.Location,
                    OrganizerName = ue.Event.Organizer.UserName
                })
                .ToListAsync();

            return Ok(participations);
        }
    }
}
