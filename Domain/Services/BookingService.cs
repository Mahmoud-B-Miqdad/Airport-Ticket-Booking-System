using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AirportTicketBookingSystem.Domain.Models;
using AirportTicketBookingSystem.Domain.Repositories;

namespace AirportTicketBookingSystem.Domain.Services
{
    public class BookingService
    {
        private readonly BookingRepository _bookingRepository;
        private readonly FlightService _Flights;

        public BookingService()
        {
            _bookingRepository = new BookingRepository();
            _Flights = new FlightService();
        }

        public void AddBooking(Booking booking)
        {
            _bookingRepository.AddBooking(booking);
            
        }

        public List<Booking> GetBookingsByPassenger(int passengerId)
        {
            return _bookingRepository.GetBookingsByPassenger(passengerId);
        }

        public void CancelBooking(int bookingId)
        {
            _bookingRepository.CancelBooking(bookingId);
            
        }

        public List<Booking> GetAllBookings()
        {
            return _bookingRepository.GetAllBookings();
        }

        public Booking GetLastBooking()
        {
            return _bookingRepository.GetLastBooking();
        }

        public void ModifyBooking(int bookingId, int newFlightId, string newSeatClass)
        {
            var booking = _bookingRepository.GetBookingById(bookingId);
            if (booking == null)
            {
                Console.WriteLine("Booking not found.");
                return;
            }

            booking.FlightId = newFlightId;
            booking.SeatClass = newSeatClass;
            _bookingRepository.UpdateBooking(booking);

            Console.WriteLine($"Booking ID {bookingId} has been updated successfully.");
        }


        public List<Booking> FilterBookings(
        double? price = null,
        string departureCountry = null,
        string destinationCountry = null,
        DateTime? departureDate = null,
        string departureAirport = null,
        string arrivalAirport = null,
        string passenger = null,
        string seatClass = null)
        {
            List<Flight> filteredFlights = _Flights.SearchFlights(
                departureCountry: departureCountry,
                destinationCountry: destinationCountry,
                departureAirport: departureAirport,
                arrivalAirport: arrivalAirport,
                departureDate: departureDate,
                seatClass: seatClass,
                maxPrice: price
            );

            var bookings = _bookingRepository.GetAllBookings();

            // For Sure that the FilterBookings function is work
            Console.WriteLine("\n\nFiltered Flights:");
            foreach (var flight in filteredFlights)
            {
                Console.WriteLine($"FlightId: {flight.Id}, Departure: {flight.DepartureCountry}, Destination: {flight.DestinationCountry}");
            }

            Console.WriteLine("All Bookings:");
            foreach (var booking in bookings)
            {
                Console.WriteLine($"Booking: {booking.Id}, FlightId: {booking.FlightId}, Passenger: {booking.Passenger.Name}, Class: {booking.SeatClass}");
            }
            Console.WriteLine("\n\n");


            // Start Filtering
            var filteredBookings = bookings.Where(b =>
                filteredFlights.Any(f => f.Id == b.FlightId) &&
                (price == null || _Flights.GetPriceByClass(filteredFlights.FirstOrDefault(f => f.Id == b.FlightId), seatClass) <= price.Value) &&
                (string.IsNullOrEmpty(passenger) || b.Passenger.Name.Equals(passenger, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(seatClass) || b.SeatClass.Equals(seatClass, StringComparison.OrdinalIgnoreCase))
            ).ToList();

            return filteredBookings;
        }
    }
}
