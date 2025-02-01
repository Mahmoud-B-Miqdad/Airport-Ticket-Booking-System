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
                        //SearchFlights();
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
            if  (flightId == null)
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



            Console.WriteLine("Your Bookings:");
            foreach (var booking in bookings)
            {
                Console.WriteLine($"ID: {booking.Id}, Flight ID: {booking.FlightId}, Class: {booking.SeatClass}, Date: {booking.BookDate}");
            }

            Console.WriteLine("1. Cancel a Booking");
            Console.WriteLine("2. Modify a Booking");
            Console.WriteLine("3. Back to Main Menu");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("Enter booking ID to cancel:");
                    int cancelId = int.Parse(Console.ReadLine());
                    _bookingService.CancelBooking(cancelId);
                    Console.WriteLine("Booking cancelled successfully.");

                    break;

                case "2":
                    Console.WriteLine("Enter booking ID to modify:");
                    int modifyId = int.Parse(Console.ReadLine());
                    Console.WriteLine("Enter FlightID:");
                    int flightid = int.Parse(Console.ReadLine());
                    Console.WriteLine("Enter new class (Economy, Business, First Class):");
                    string newClass = Console.ReadLine();
                    _bookingService.ModifyBooking(modifyId, flightid, newClass);
                    Console.WriteLine("Booking modified successfully.");

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