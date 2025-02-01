using AirportTicketBookingSystem.Domain.Models;
using AirportTicketBookingSystem.Domain.Services;
using System;

namespace AirportTicketBookingSystem.Utilities
{
    public class UserInteraction
    {
        private readonly FlightService _flightService;
        private readonly BookingService _bookingService;

        public UserInteraction()
        {
            _flightService = new FlightService();
            _bookingService = new BookingService();
        }

        private void MainMenu()
        {
            Console.WriteLine("\nPlease select an option:");
            Console.WriteLine("1. Book a Flight");
            Console.WriteLine("2. Search for Available Flights");
            Console.WriteLine("3. Manage Bookings");
            Console.WriteLine("4. Exit");
        }
        public void Start()
        {
            Console.WriteLine("Welcome to the Airport Ticket Booking System!");
            bool exit = false;

            while (!exit)
            {
                MainMenu();

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        BookFlight();
                        break;
                    case "2":
                        SearchAndDisplayFlights();
                        break;
                    case "3":
                        ManageBookings();
                        break;
                    case "4":
                        exit = true;
                        Console.WriteLine("Thank you for using the system. Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        private int? GetFlightId()
        {
            Console.WriteLine("Enter the flight ID you want to book:");
            int flightId = int.Parse(Console.ReadLine());

            if (_flightService.GetFlightById(flightId) == null)
            {
                Console.WriteLine("Flight Not Found:");
                return null;
            }

            return flightId;
        }

        private Passenger PassengerInfo()
        {
            Console.WriteLine("Enter your name:");
            string name = Console.ReadLine();

            Console.WriteLine("Enter your email:");
            string email = Console.ReadLine();

            Console.WriteLine("Enter your phone number:");
            string phoneNumber = Console.ReadLine();

            var passenger = new Passenger
            {
                Id = new Random().Next(1000, 9999),
                Name = name,
                Email = email,
            };

            return passenger;
        }

        private string ValidateSeatClass()
        {
            string seatClass;
            while (true)
            {
                Console.Write("Class (Economy, Business, First Class): ");
                seatClass = Console.ReadLine().Trim();

                if (string.IsNullOrEmpty(seatClass) ||
                    seatClass.Equals("Economy", StringComparison.OrdinalIgnoreCase) ||
                    seatClass.Equals("Business", StringComparison.OrdinalIgnoreCase) ||
                    seatClass.Equals("First Class", StringComparison.OrdinalIgnoreCase))
                {
                    return seatClass;
                }
                Console.WriteLine("Invalid class. Please enter 'Economy', 'Business', or 'First Class'.");
            }
        }

        private void BookFlight()
        {
            int? flightId = GetFlightId();
            if (flightId == null)
                return;

            var passenger = PassengerInfo();

            string seatClass = ValidateSeatClass();

            var booking = new Booking
            {
                Id = new Random().Next(100, 999),
                passenger = passenger,
                FlightId = flightId,
                SeatClass = seatClass,
                BookDate = DateTime.Now
            };


            _bookingService.AddBooking(booking);
            Console.WriteLine($"Booking for {booking.passenger.Id} added successfully.");
        }


        private void SearchAndDisplayFlights()
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

            string seatClass = ValidateSeatClass();

            Console.Write("Enter Maximum Price or leave empty: ");
            string priceInput = Console.ReadLine()?.Trim();
            double? maxPrice = string.IsNullOrEmpty(priceInput) ? null : double.Parse(priceInput);

            var flights = _flightService.SearchFlights(departureCountry, destinationCountry, departureAirport, arrivalAirport, departureDate, seatClass, maxPrice);

            if (flights.Any())
            {
                Console.WriteLine("\nAvailable Flights:");
                foreach (var flight in flights)
                {
                    Console.WriteLine($"Flight ID: {flight.Id}, From {flight.DepartureCountry} ({flight.DepartureAirport}) to {flight.DestinationCountry} ({flight.ArrivalAirport}), Date: {flight.DepartureDate}, Price: {flight.Prices[seatClass.ToLower()]:C}");
                }
            }
            else
            {
                Console.WriteLine("\nNo flights found matching your criteria.");
            }
        }


        private void ManageMenue()
        {
            Console.WriteLine("1. Cancel a Booking");
            Console.WriteLine("2. Modify a Booking");
            Console.WriteLine("3. Back to Main Menu");
        }

        private void CancelBooking()
        {
            Console.WriteLine("Enter booking ID to cancel:");
            int cancelId = int.Parse(Console.ReadLine());
            _bookingService.CancelBooking(cancelId);
        }

        private void ModifyBooking()
        {
            Console.WriteLine("Enter booking ID to modify:");
            int modifyId = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter FlightID:");
            int flightid = int.Parse(Console.ReadLine());

            string newClass = ValidateSeatClass();

            _bookingService.ModifyBooking(modifyId, flightid, newClass);
        }


        private void ManageBookings()
        {
            Console.WriteLine("Enter your passenger ID:");
            int passengerId = int.Parse(Console.ReadLine());

            var bookings = _bookingService.GetBookingsByPassenger(passengerId);

            if (bookings == null)
            {
                Console.WriteLine("passenger Not have booking:");
                return;
            }

            Console.WriteLine("Your Bookings:\r\n");
            foreach (var booking in bookings)
            {
                Console.WriteLine($"ID: {booking.Id}, Flight ID: {booking.FlightId}, Class: {booking.SeatClass}, Date: {booking.BookDate}\r\n");
            }

            ManageMenue();

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    CancelBooking();
                    break;

                case "2":
                    ModifyBooking();
                    break;

                case "3":
                    break;

                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }
}