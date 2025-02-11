using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AirportTicketBookingSystem.Domain.Models;
using AirportTicketBookingSystem.Domain.Repositories;
using AirportTicketBookingSystem.Utilities;

namespace AirportTicketBookingSystem.Domain.Services
{
    public class BookingService
    {
        private readonly BookingRepository _bookingRepository;
        private readonly FlightService _flightsService;
        public BookingService()
        {
            _bookingRepository = new BookingRepository();
            _flightsService = new FlightService();
        }

        private void AddBooking(Booking booking)
        {
            _bookingRepository.AddBooking(booking);
            
        }


        public void BookFlight(int? flightId,Passenger passenger, string seatClass)
        {

            var flight = new Flight
            {
                Id = flightId
            };
            var booking = new Booking
            {
                Id = ++BookingHandler.BookingId,
                Passenger = passenger,
                Flight = flight,
                SeatClass = seatClass,
                BookDate = DateTime.Now
            };

            AddBooking(booking);
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

            booking.Flight.Id = newFlightId;
            booking.SeatClass = newSeatClass;
            _bookingRepository.UpdateBooking(booking);

        }


        public List<(Booking Booking, Flight Flight)> FilterBookings(
        double? price = null,
        string departureCountry = null,
        string destinationCountry = null,
        DateTime? departureDate = null,
        string departureAirport = null,
        string arrivalAirport = null,
        string passenger = null,
        string seatClass = null)
        {
            List<Flight> filteredFlights = _flightsService.SearchFlights(
                departureCountry: departureCountry,
                destinationCountry: destinationCountry,
                departureAirport: departureAirport,
                arrivalAirport: arrivalAirport,
                departureDate: departureDate,
                seatClass: seatClass,
                maxPrice: price
            );

            return _bookingRepository.FilteredBookings(filteredFlights, price, seatClass, passenger);
        }
    }
}
