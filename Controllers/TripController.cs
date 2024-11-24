using Microsoft.AspNetCore.Mvc;
using TripPlanner.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace TripPlanner.Controllers
{
    public class TripController : Controller
    {
        private readonly TripContext _context;

        // Constructor to initialize the context
        public TripController(TripContext context)
        {
            _context = context;
        }

        // Action to start a new trip (GET)
        public IActionResult Start()
        {
            // Initialize an empty Trip object
            var trip = new Trip();
            return View(trip); // Pass the empty trip to the view
        }

        // Action to start a new trip (POST)
        [HttpPost]
        public IActionResult Start(Trip trip)
        {
            if (ModelState.IsValid)  // Check if the model is valid
            {
                // Ensure that the Accommodations field is not null
                if (string.IsNullOrWhiteSpace(trip.Accommodations))
                {
                    // Set Accommodations to a default value if it's empty
                    trip.Accommodations = "Not specified";
                }

                // Store the trip details in TempData for the next steps
                TempData["Destination"] = trip.Destination;
                TempData["StartDate"] = trip.StartDate?.ToString("yyyy-MM-dd");  // Store as string in TempData
                TempData["EndDate"] = trip.EndDate?.ToString("yyyy-MM-dd");      // Store as string in TempData
                TempData["Accommodations"] = trip.Accommodations;

                _context.Trips.Add(trip);
                _context.SaveChanges();

                TempData["Message"] = "Redirecting to accommodation details";
                return RedirectToAction("AccommodationDetails");

            }

            // If the model is invalid, log the errors for debugging
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine("Model Error: " + error.ErrorMessage);
            }

            // If the model is invalid, return the same view with the model data
            return View(trip);
        }


        // Action to add accommodation details (GET)
        public IActionResult AccommodationDetails()
        {
            // Access TempData to get the previously stored data
            ViewBag.Destination = TempData["Destination"];
            ViewBag.StartDate = TempData["StartDate"];
            ViewBag.EndDate = TempData["EndDate"];
            ViewBag.Accommodations = TempData["Accommodations"];

            // Pass the accommodation info (if any) from TempData to the view
            ViewBag.Subhead = TempData.Peek("Accommodations");
            return View();
        }

        // Action to add accommodation details (POST)
        [HttpPost]
        public IActionResult AccommodationDetails(string accommodationDetails)
        {
            // Store the accommodation details in TempData
            TempData["AccommodationDetails"] = accommodationDetails;
            return RedirectToAction("ThingsToDo");
        }

        // Action to add things to do (GET)
        public IActionResult ThingsToDo()
        {
            // Pass the destination info (if any) from TempData to the view
            ViewBag.Subhead = TempData.Peek("Destination");
            return View();
        }

        // Action to add things to do (POST)
        [HttpPost]
        public IActionResult ThingsToDo(string thingsToDo)
        {
            // Create a new Trip object using TempData values
            var trip = new Trip
            {
                Destination = TempData["Destination"]?.ToString(),
                StartDate = TempData["StartDate"] != null ? DateTime.Parse(TempData["StartDate"].ToString()) : (DateTime?)null,
                EndDate = TempData["EndDate"] != null ? DateTime.Parse(TempData["EndDate"].ToString()) : (DateTime?)null,

                // Get the Accommodations value from TempData, fallback to an empty string or null
                Accommodations = TempData["Accommodations"]?.ToString() ?? string.Empty, // Ensure it doesn't insert NULL into DB
                AccommodationDetails = TempData["AccommodationDetails"]?.ToString(),
                ThingsToDo = thingsToDo
            };

            // Optional: Check if Accommodations is empty and set it to null if required by your schema
            if (string.IsNullOrEmpty(trip.Accommodations))
            {
                trip.Accommodations = null;  // Set to null if empty
            }

            // Add the new trip to the database
            _context.Trips.Add(trip);
            _context.SaveChanges();  // Save the changes to the database

            // Set a success message in TempData and redirect to the Home page
            TempData["Message"] = "Trip added successfully!";
            return RedirectToAction("Index", "Home");
        }
    }
}
