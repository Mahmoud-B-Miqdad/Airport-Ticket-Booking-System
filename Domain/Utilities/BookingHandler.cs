using System;
using System.Linq;
using AirportTicketBookingSystem.Domain.Models;
using AirportTicketBookingSystem.Domain.Services;

namespace AirportTicketBookingSystem.Utilities
{
    public class BookingHandler
    {
        private readonly BookingService _bookingService;
        private readonly FlightService _flightService;
        public static int PassengerId;
        public static int BookingId;


        public BookingHandler()
        {
            try
            {
                _bookingService = new BookingService();
                _flightService = new FlightService();
                var LastBooking = _bookingService.GetLastBooking();
                PassengerId = LastBooking?.Passenger?.Id ?? 0;
                BookingId = LastBooking?.Id ?? 0;
            }
            catch (ApplicationException ex)
            {
                Console.WriteLine($"\nError: {ex.Message} Please check the flight data file and correct any errors before running the program again.");
                Environment.Exit(1);
            }

            
        }

        private void ReadKey()
        {
            Console.WriteLine("Press Enter to back");
            Console.ReadLine();
        }

        public void BookFlight()
        {
            var flights = _flightService.GetAllFlights();
            if (!flights.Any())
            {
                Console.WriteLine("No flights available.");
                return;
            }

            Console.WriteLine("\nAvailable Flights:");
            foreach (var flight in flights)
            {
                Console.WriteLine($"Flight ID: {flight.Id}, From ({flight.DepartureCountry}) to ({flight.DestinationCountry}) in Date: {flight.DepartureDate}");
            }

            int? flightId = InputHelper.GetIntegerInput("Enter the flight ID you want to book:");

            if (flightId == null || _flightService.GetFlightById(flightId.Value) == null)
            {
                Console.WriteLine("Invalid flight ID.");
                return;
            }

            var passenger = InputHelper.GetPassengerInfo(++BookingHandler.PassengerId);
            SeatClass seatClass = InputHelper.ValidateSeatClass();

            _bookingService.BookFlight(flightId, passenger, seatClass);
            Console.WriteLine($"Booking added successfully. Your ID is: {PassengerId}.");
        }


        private void CancelBooking()
        {
            int cancelId = InputHelper.GetIntegerInput("Enter booking ID to cancel:") ?? 0;

            try
            {
                _bookingService.CancelBooking(cancelId);
                Console.WriteLine($"Booking for {cancelId} canceled successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while canceling the booking: {ex.Message}");
            }
        }



        private void ModifyBooking()
        {
            int modifyId = InputHelper.GetIntegerInput("Enter booking ID to modify:") ?? 0;
            int flightId = InputHelper.GetIntegerInput("Enter new Flight ID:") ?? 0;
            SeatClass newClass = InputHelper.ValidateSeatClass();
            _bookingService.ModifyBooking(modifyId, flightId, newClass);
            Console.WriteLine($"Booking ID {modifyId} has been updated successfully.");
        }


        public void ManageBookings()
        {
            int passengerId = InputHelper.GetIntegerInput("Enter your passenger ID:") ?? 0;
            var bookings = _bookingService.GetBookingsByPassenger(passengerId);

            if (bookings == null || bookings.Count == 0)
            {
                Console.WriteLine("No bookings found.");
                return;
            }

            foreach (var booking in bookings)
            {
                Console.WriteLine($"ID: {booking.Id}, Flight ID: {booking.Flight.Id}, Class: {booking.SeatClass}, Date: {booking.BookDate}");
            }

            Console.WriteLine("1. Cancel a Booking\n2. Modify a Booking\n3. Back to Main Menu");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    CancelBooking();
                    break;

                case "2":
                    try
                    {
                        ModifyBooking();
                    }
                    catch (KeyNotFoundException ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                    }
                    break;

                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }
}
