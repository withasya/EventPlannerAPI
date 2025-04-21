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
    [Authorize]
    public class ParticipationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ParticipationController(ApplicationDbContext context)
        {
            _context = context;
        }


        //Kullanıcı etkinliğe katılıyor
        // POST: api/participation
        [HttpPost]
        public async Task<IActionResult> JoinEvent([FromBody] int eventId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Etkinlik var mı ve onaylanmış mı?
            var @event = await _context.Events
                .FirstOrDefaultAsync(e => e.Id == eventId && e.Status == EventStatus.Approved);

            if (@event == null)
                return BadRequest("Etkinlik bulunamadı ya da henüz onaylanmamış.");

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

            var participations = await _context.UserEvents
                .Where(ue => ue.UserId == userId)
                .Include(ue => ue.Event)
                .ThenInclude(e => e.Organizer)
                .ToListAsync();

            return Ok(participations);
        }
    }
}
