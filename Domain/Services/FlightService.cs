using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AirportTicketBookingSystem.Domain.Models;
using AirportTicketBookingSystem.Domain.Repositories;

namespace AirportTicketBookingSystem.Domain.Services
{
    public class FlightService
    {
        private readonly FlightRepository _flightRepository;

        public FlightService()
        {
            _flightRepository = new FlightRepository();
        }

        public void AddFlight(Flight flight)
        {
            _flightRepository.AddFlight(flight);
        }

        public List<Flight> SearchFlights(
            string departureCountry = "",
            string destinationCountry = "",
            string departureAirport = "",
            string arrivalAirport = "",
            DateTime? departureDate = null,
            string seatClass = "",
            double? maxPrice = null)
        {
            var flights = _flightRepository.GetAllFlights();
            return flights.Where(f =>
                (string.IsNullOrEmpty(departureCountry) || f.DepartureCountry.Equals(departureCountry, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(destinationCountry) || f.DestinationCountry.Equals(destinationCountry, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(departureAirport) || f.DepartureAirport.Equals(departureAirport, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(arrivalAirport) || f.ArrivalAirport.Equals(arrivalAirport, StringComparison.OrdinalIgnoreCase)) &&
                (!departureDate.HasValue || f.DepartureDate.Date == departureDate.Value.Date) &&
                (maxPrice == null || GetPriceByClass(f, seatClass) <= maxPrice.Value)
            ).ToList();
        }

        public List<Flight> GetAllFlights()
        {
            return _flightRepository.GetAllFlights();
        }

        public Flight GetFlightById(int? flightId)
        {
            return _flightRepository.GetFlightById(flightId);
        }

        public double GetPriceByClass(Flight flight, string seatClass)
        {
            if (flight.Prices.TryGetValue(seatClass.ToLower(), out double price))
            {
                return price;
            }
            return flight.Prices.ContainsKey("c") ? flight.Prices["economy"] : 0; 
        }
    }
}
