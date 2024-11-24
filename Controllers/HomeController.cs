using Microsoft.AspNetCore.Mvc;
using TripPlanner.Models;
using System.Linq;

namespace TripPlanner.Controllers
{
    public class HomeController : Controller
    {
        private readonly TripContext _context;

        public HomeController(TripContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Fetch all trips from the database
            var trips = _context.Trips.ToList();

            // Set the message from TempData (if it exists)
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }

            // Pass the list of trips directly to the view
            return View(trips);
        }
    }
}
