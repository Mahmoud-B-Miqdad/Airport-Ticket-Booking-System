using AirportTicketBookingSystem.Domain.Models;
using AirportTicketBookingSystem.Domain.Repositories;
using AirportTicketBookingSystem.Domain.Services;
using AirportTicketBookingSystem.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AirportTicketBookingSystem.Domain.Utilities
{
    public class AdminHandler
    {
        private readonly BookingService _bookingService;
        private readonly FlightService _flightService;
        private readonly CSVFlightImporter _csvFlightImporter;

        public AdminHandler()
        {
            try
            {
                _bookingService = new BookingService();
                _flightService = new FlightService();
            }
            catch (ApplicationException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            _csvFlightImporter = new CSVFlightImporter();
        }

        private void ReadKey()
        {
            Console.WriteLine("Press Enter to back");
            Console.ReadLine();
        }

        private void AdminMenu()
        {
            Console.Clear();
            Console.WriteLine("\n=====================");
            Console.WriteLine("   Admin Options");
            Console.WriteLine("=====================");
            Console.WriteLine("1. Search and Display All Bookings");
            Console.WriteLine("2. Import Flights from CSV");
            Console.WriteLine("3. Exit Admin Options");
            Console.WriteLine("=====================");
        }

        public void AdminOptions()
        {
            if (!AuthenticateUser()) return;

            bool exit = false;
            while (!exit)
            {
                AdminMenu();

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        SearchAndDisplayBookings();
                        ReadKey();
                        break;
                    case "2":
                        ImportFlightsFromCsv();
                        ReadKey();
                        break;
                    case "3":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        private bool AuthenticateUser()
        {
            string correctUsername = "admin";
            string correctPassword = "1234";

            Console.WriteLine("Please enter your username:");
            string username = Console.ReadLine();

            Console.WriteLine("Please enter your password:");
            string password = Console.ReadLine();

            if (username == correctUsername && password == correctPassword)
            {
                Console.WriteLine("Authentication successful!");
                return true;
            }
            else
            {
                Console.WriteLine("Invalid username or password. Please try again.");
                return false;
            }
        }

        private void SearchAndDisplayBookings()
        {
            Console.Write("Enter Departure Country: ");
            string departureCountry = Console.ReadLine()?.Trim();

            Console.Write("Enter Destination Country: ");
            string destinationCountry = Console.ReadLine()?.Trim();

            Console.Write("Enter Departure Airport: ");
            string departureAirport = Console.ReadLine()?.Trim();

            Console.Write("Enter Arrival Airport: ");
            string arrivalAirport = Console.ReadLine()?.Trim();

            Console.Write("Enter Departure Date (yyyy-MM-dd) or leave empty: ");
            string dateInput = Console.ReadLine()?.Trim();
            DateTime? departureDate = string.IsNullOrEmpty(dateInput) ? null : DateTime.Parse(dateInput);

            Console.Write("Enter Passenger Name or leave empty: ");
            string passenger = Console.ReadLine()?.Trim();

            SeatClass seatClass = InputHelper.ValidateSeatClass();

            Console.Write("Enter Maximum Price or leave empty: ");
            string priceInput = Console.ReadLine()?.Trim();
            double? maxPrice = string.IsNullOrEmpty(priceInput) ? null : double.Parse(priceInput);

            var bookings = _bookingService.FilterBookings(maxPrice, departureCountry, destinationCountry, departureDate, departureAirport, arrivalAirport, passenger, seatClass);


            if (bookings.Any())
            {
                Console.WriteLine("\nFiltered Bookings based on your search:");
                foreach (var (booking, flight) in bookings)
                {
                    double bookingPrice = flight != null ? _flightService.GetPriceByClass(flight, booking.SeatClass) : 0.0;

                    Console.WriteLine($"Booking ID: {booking.Id}, Passenger: {booking.Passenger.Name}, " +
                                      $"Flight ID: {booking.Flight.Id}, Seat Class: {booking.SeatClass}, Price: {bookingPrice:C}");
                }
            }
            else
            {
                Console.WriteLine("\nNo bookings found matching your criteria.");
            }

        }

        private void ImportFlightsFromCsv()
        {
            var flights = _csvFlightImporter.ImportFlights();

            if (_csvFlightImporter.Errors.Count > 0)
            {
                Console.WriteLine("\n\nErrors occurred during import:");
                foreach (var error in _csvFlightImporter.Errors)
                {
                    Console.WriteLine(error);
                }
            }
            Console.WriteLine("\n");

            foreach (var flight in flights)
            {
                Console.WriteLine($"Flight ID: {flight.Id}, From ({flight.DepartureCountry}) to ({flight.DestinationCountry}) in Date: {flight.DepartureDate}");
            }

            Console.WriteLine($"\n{flights.Count} flights imported successfully.");
        }
    }
}