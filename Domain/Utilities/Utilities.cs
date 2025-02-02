using AirportTicketBookingSystem.Domain.Models;
using AirportTicketBookingSystem.Domain.Services;
using System;

namespace AirportTicketBookingSystem.Utilities
{
    public class UserInteraction
    {
        private readonly FlightService _flightService;
        private readonly BookingService _bookingService;
        private static int PassengerId;
        private static int BookingId;

        public UserInteraction()
        {
            _flightService = new FlightService();
            _bookingService = new BookingService();

            var LastBooking = _bookingService.GetLastBooking();
            PassengerId = LastBooking.passenger.Id;
            BookingId = LastBooking.Id;
        }

        private void ReadKey()
        {
            Console.WriteLine("Press Enter to back");
            Console.ReadKey();
        }

        private void MainMenu()
        {
            Console.WriteLine("Welcome to the Airport Ticket Booking System!");


            Console.WriteLine("\n=====================");
            Console.WriteLine("   Main Menu");
            Console.WriteLine("=====================");
            Console.WriteLine("1. Book a Flight");
            Console.WriteLine("2. Search for Available Flights");
            Console.WriteLine("3. Manage My Bookings");
            Console.WriteLine("4. Admin Options");
            Console.WriteLine("5. Exit");
            Console.WriteLine("=====================");
        }

        public void Start()
        {

            bool exit = false;

            while (!exit)
            {

                Console.Clear();

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
                        AdminOptions();
                        break;
                    case "5":
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
            Console.WriteLine("\nEnter the flight ID you want to book:");
            int flightId = int.Parse(Console.ReadLine());

            if (_flightService.GetFlightById(flightId) == null)
            {
                Console.WriteLine("\nFlight Not Found:");
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
                Id = ++PassengerId,
                Name = name,
                Email = email,
            };

            return passenger;
        }

        private string ValidateSeatClass()
        {
            string seatClass = "";
            while (true)
            {
                Console.Write("Class (Economy, Business, First Class): ");
                seatClass = Console.ReadLine().Trim();

                if (seatClass == "")
                {
                    Console.WriteLine("Invalid class. Please enter 'Economy', 'Business', or 'First Class'.");
                    continue;
                }

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

            var flights = _flightService.GetAllFlights();

            Console.WriteLine("\nAvailable Flights:");
            foreach (var flight in flights)
            {
                Console.WriteLine($"Flight ID: {flight.Id}, From ({flight.DepartureCountry}) to ({flight.DestinationCountry}) in Date: {flight.DepartureDate}");
            }

            int? flightId = GetFlightId();
            if (flightId == null)
                return;

            var passenger = PassengerInfo();
            string seatClass = ValidateSeatClass();

            var booking = new Booking
            {
                Id = ++BookingId,
                passenger = passenger,
                FlightId = flightId,
                SeatClass = seatClass,
                BookDate = DateTime.Now
            };


            _bookingService.AddBooking(booking);
            Console.WriteLine($"Booking added successfully.");
            Console.WriteLine($"Your Id is: {booking.passenger.Id}.");

            ReadKey();
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
                Console.WriteLine("\nAvailable Flights based on your search:");
                foreach (var flight in flights)
                {
                    Console.WriteLine($"Flight ID: {flight.Id}, From {flight.DepartureCountry} ({flight.DepartureAirport}) to {flight.DestinationCountry} ({flight.ArrivalAirport}), Date: {flight.DepartureDate}, Price: {flight.Prices[seatClass.ToLower()]:C}");
                }

            }
            else
            {
                Console.WriteLine("\nNo flights found matching your criteria.");
            }

            ReadKey();
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

            ReadKey();
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

            string seatClass = ValidateSeatClass();

            Console.Write("Enter Maximum Price or leave empty: ");
            string priceInput = Console.ReadLine()?.Trim();
            double? maxPrice = string.IsNullOrEmpty(priceInput) ? null : double.Parse(priceInput);

            var bookings = _bookingService.FilterBookings(maxPrice, departureCountry, destinationCountry, departureDate, departureAirport, arrivalAirport, passenger, seatClass);

            if (bookings.Any())
            {
                Console.WriteLine("\nFiltered Bookings based on your search:");
                foreach (var booking in bookings)
                {
                    var flight = _flightService.GetFlightById(booking.FlightId);
                    double price = flight != null ? _flightService.GetPriceByClass(flight, booking.SeatClass) : 0.0;

                    Console.WriteLine($"Booking ID: {booking.Id}, Passenger: {booking.passenger.Name}, Flight ID: {booking.FlightId}, Seat Class: {booking.SeatClass}, Price: {price:C}");
                }
            }
            else
            {
                Console.WriteLine("\nNo bookings found matching your criteria.");
            }

        }


        private void AdminMenu()
        {
            Console.WriteLine("\n=====================");
            Console.WriteLine("   Admin Options");
            Console.WriteLine("=====================");
            Console.WriteLine("1. Search and Display All Bookings");
            Console.WriteLine("2. Exit Admin Options");
            Console.WriteLine("=====================");
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


        private void AdminOptions()
        {

            Console.Clear();

            bool isAuthenticated = false;
            while (!isAuthenticated)
            {
                isAuthenticated = AuthenticateUser();
            }


            bool exit = false;
            while (!exit)
            {
                Console.Clear();

                AdminMenu();

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        SearchAndDisplayBookings();
                        break;
                    case "2":
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
                break;
            }

            ReadKey();
        }

    }
}