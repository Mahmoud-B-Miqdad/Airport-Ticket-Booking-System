using AirportTicketBookingSystem.Domain.Utilities;
using System;

namespace AirportTicketBookingSystem.Utilities
{
    public class MainMenuHandler
    {
        private readonly BookingHandler _bookingHandler;
        private readonly FlightSearchHandler _flightSearchHandler;
        private readonly AdminHandler _adminHandler;

        public MainMenuHandler()
        {
            _bookingHandler = new BookingHandler();
            _flightSearchHandler = new FlightSearchHandler();
            _adminHandler = new AdminHandler();
        }

        private void ReadKey()
        {
            Console.WriteLine("Press Enter to back");
            Console.ReadLine();
        }

        public void DisplayMainMenu()
        {
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
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

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        _bookingHandler.BookFlight();
                        ReadKey();
                        break;
                    case "2":
                        _flightSearchHandler.SearchAndDisplayFlights();
                        ReadKey();
                        break;
                    case "3":
                        _bookingHandler.ManageBookings();
                        ReadKey();
                        break;
                    case "4":
                        _adminHandler.AdminOptions();
                        ReadKey();
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
    }
}
