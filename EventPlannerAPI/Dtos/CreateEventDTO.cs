using System.ComponentModel.DataAnnotations;

namespace EventPlannerAPI.Dtos
{
    public class CreateEventDTO
    {
        [Required(ErrorMessage = "Etkinlik başlığı zorunludur")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Etkinlik konumu zorunludur")]
        public string? Location { get; set; }
    }
}