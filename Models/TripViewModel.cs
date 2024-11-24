using System.Collections.Generic;

namespace TripPlanner.Models
{
    public class TripViewModel
    {
        public List<Trip> Trips { get; set; }  // Add this property to store a list of trips
        public string Destination { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Accommodations { get; set; }
        public string AccommodationDetails { get; set; }
        public string ThingsToDo { get; set; }
    }
}
