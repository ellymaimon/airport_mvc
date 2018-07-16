using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Airport.Models;
using Microsoft.AspNetCore.Mvc;


namespace Airport.Controllers
{
    public class FlightsController : Controller
    {
        [HttpGet("/flights")]
        public ActionResult Index()
        {
            List<Flight> allFlights = Flight.GetAll();
            return View(allFlights);
        }

        [HttpGet("/flights/create")]
        public ActionResult DepartureForm()
        {
            List<City> cities = City.GetAll();

            return View(cities);
        }

        [HttpPost("/flights/create")]
        public ActionResult Create(int arrivalCity, int departureCity, TimeSpan arrivalTime, TimeSpan departureTime)
        {
            int flightCode = Flight.GetCode();
            Flight departureFlight = new Flight(flightCode, "Departure", departureTime, "On Time", 0);
            Flight arrivalFlight = new Flight(flightCode, "Arrival", arrivalTime, "On Time", 0);
            departureFlight.Save();
            arrivalFlight.Save();

            City departure = City.Find(departureCity);
            departureFlight.AddCity(departure);
            City arrival = City.Find(arrivalCity);
            arrivalFlight.AddCity(arrival);

            return RedirectToAction("Index");
        }
    }
}
