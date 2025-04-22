using EventPlannerAPI.Models;

namespace EventPlannerAPI.Dtos
{
    public class EventDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public string OrganizerName { get; set; }
        public string Status { get; set; }
    }
}