namespace EventPlannerAPI.Models
{
    public class UserEvent
    {
        public string UserId { get; set; }
        public int EventId { get; set; }

        public ApplicationUser User { get; set; }
        public Event Event { get; set; }
    }
}

