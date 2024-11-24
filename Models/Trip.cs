using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TripPlanner.Models
{
    public class Trip
    {
        [Key]
        [Column("TripId")]  // Map to the database column 'TripId'
        public int Id { get; set; }  // This will be the property that holds the TripId

        [Required]
        public string? Destination { get; set; }  // Marked as nullable

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "Accommodations is required")]
        public string Accommodations { get; set; }
        public string AccommodationDetails { get; set; } = string.Empty;  // Default value
        public string ThingsToDo { get; set; } = string.Empty;
    }
}
