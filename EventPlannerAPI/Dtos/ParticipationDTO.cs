using EventPlannerAPI.Models;

namespace EventPlannerAPI.Dtos
{
    public class ParticipationDTO
    {
        public int EventId { get; set; }
        public string EventTitle { get; set; }
        public string EventLocation { get; set; }
        public string OrganizerName { get; set; }
    }
}