using Microsoft.AspNetCore.Identity;

namespace EventPlannerAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Kullanıcının organize ettiği etkinlikler
        public ICollection<Event> Events { get; set; }

        // Kullanıcının katıldığı etkinlikler (UserEvent)
        public ICollection<UserEvent> UserEvents { get; set; }
    }
}
