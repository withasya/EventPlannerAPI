using System;
using System.ComponentModel.DataAnnotations;

namespace EventPlannerAPI.Models
{
    public class Event
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Location { get; set; }

        public string OrganizerId { get; set; }  // Kullanıcı ID
        public ApplicationUser Organizer { get; set; }  // Kullanıcı (IdentityUser)

        public ICollection<UserEvent> UserEvents { get; set; } // Etkinlikteki katılımcı kullanıcılar (UserEvent)

        public EventStatus Status { get; set; } = EventStatus.Pending;  // Varsayılan durum "Pending"
    }

    public enum EventStatus
    {
        Pending,   // Onay Bekliyor
        Approved,  // Onaylandı
        Rejected   // Reddedildi
    }
}
