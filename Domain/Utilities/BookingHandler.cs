using System;
using AirportTicketBookingSystem.Domain.Models;
using AirportTicketBookingSystem.Domain.Services;

namespace AirportTicketBookingSystem.Utilities
{
    public class BookingHandler
    {
        private readonly BookingService _bookingService;
        private readonly FlightService _flightService;
        private static int PassengerId;
        private static int BookingId;


        public BookingHandler()
        {
            _bookingService = new BookingService();
            _flightService = new FlightService();
            var LastBooking = _bookingService.GetLastBooking();
            PassengerId = LastBooking?.passenger?.Id ?? 0;
            BookingId = LastBooking?.Id ?? 0;
        }

        private void ReadKey()
        {
            Console.WriteLine("Press Enter to back");
            Console.ReadLine();
        }

        public void BookFlight()
        {
            var flights = _flightService.GetAllFlights();
            if (flights.Count == 0)
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

            var passenger = InputHelper.GetPassengerInfo(++PassengerId);
            string seatClass = InputHelper.ValidateSeatClass();

            var booking = new Booking
            {
                Id = ++BookingId,
                passenger = passenger,
                FlightId = flightId,
                SeatClass = seatClass,
                BookDate = DateTime.Now
            };

            _bookingService.AddBooking(booking);
            Console.WriteLine($"Booking added successfully. Your ID is: {booking.passenger.Id}.");
        }


        private void CancelBooking()
        {
            int cancelId = InputHelper.GetIntegerInput("Enter booking ID to cancel:") ?? 0;
            _bookingService.CancelBooking(cancelId);
        }


        private void ModifyBooking()
        {
            int modifyId = InputHelper.GetIntegerInput("Enter booking ID to modify:") ?? 0;
            int flightId = InputHelper.GetIntegerInput("Enter new Flight ID:") ?? 0;
            string newClass = InputHelper.ValidateSeatClass();
            _bookingService.ModifyBooking(modifyId, flightId, newClass);
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
                Console.WriteLine($"ID: {booking.Id}, Flight ID: {booking.FlightId}, Class: {booking.SeatClass}, Date: {booking.BookDate}");
            }

            Console.WriteLine("1. Cancel a Booking\n2. Modify a Booking\n3. Back to Main Menu");
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
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }
}
