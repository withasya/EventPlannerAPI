using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EventPlannerAPI.Models;

namespace EventPlannerAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Modelleri DbContext'e ekliyoruz
        public DbSet<Event> Events { get; set; }
        public DbSet<UserEvent> UserEvents { get; set; }

        // İlişkileri yapılandırıyoruz
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Event ve ApplicationUser arasındaki ilişkiyi kuruyoruz
            modelBuilder.Entity<Event>()
                .HasOne(e => e.Organizer)  // Etkinlik bir kullanıcıya (organizer) ait
                .WithMany(u => u.Events)   // Kullanıcı birden fazla etkinliğe sahip olabilir
                .HasForeignKey(e => e.OrganizerId); // Foreign key

            // UserEvent ilişkisini kuruyoruz
            modelBuilder.Entity<UserEvent>()
                .HasKey(ue => new { ue.UserId, ue.EventId }); // UserEvent için composite primary key

            modelBuilder.Entity<UserEvent>()
                .HasOne(ue => ue.User) // UserEvent bir kullanıcıya ait
                .WithMany(u => u.UserEvents) // Kullanıcı birden fazla etkinliğe katılabilir
                .HasForeignKey(ue => ue.UserId); // Foreign key

            modelBuilder.Entity<UserEvent>()
                .HasOne(ue => ue.Event) // UserEvent bir etkinliğe ait
                .WithMany(e => e.UserEvents) // Etkinlik birden fazla kullanıcıya sahip olabilir
                .HasForeignKey(ue => ue.EventId); // Foreign key
        }
    }
}
