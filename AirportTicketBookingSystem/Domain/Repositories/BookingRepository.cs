﻿using AirportTicketBookingSystem.Domain.Models;
using AirportTicketBookingSystem.Domain.Services;
using AirportTicketBookingSystem.Domain.Utilities;

namespace AirportTicketBookingSystem.Domain.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly string _filePath;
    private readonly List<Booking> _bookings;
    private readonly IFlightService _flightsService;
    private readonly IFileHandler _fileStorage;

    public BookingRepository(IFileHandler fileStorage, IFlightService flightsService)
    {
        try
        {
            string relativePath = @"Data\bookings.csv";
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);

            _bookings = new List<Booking>();
            _flightsService = flightsService;
            _fileStorage = fileStorage;

            if (_fileStorage.ReadAllLines(_filePath).Length > 0)
            {
                LoadBookings();
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"\nError parsing Booking data: {ex.Message}", ex);
        }
    }


    public void AddBooking(Booking booking)
    {
        foreach (var b in _bookings)
        {
            if (b.Id == booking.Id)
                throw new InvalidOperationException($"\nBooking {booking.Id} already exists");
        }
        _bookings.Add(booking);
        SaveBookings();
    }

    public List<Booking> GetBookingsByPassenger(int passengerId)
    {
        if (passengerId <= 0)
            throw new ArgumentException("Invalid passenger ID");

        return _bookings.Where(b => b.Passenger.Id == passengerId).ToList(); 
    }

    public Booking GetBookingById(int bookingId)
    {
        return _bookings.FirstOrDefault(b => b.Id == bookingId);
    }

    public void CancelBooking(int bookingId)
    {
        var booking = GetBookingById(bookingId);
        if (booking == null)
        {
            throw new KeyNotFoundException($"\nBooking ID {bookingId} Not Found.");
        }
        _bookings.Remove(booking);
        SaveBookings();
    }

    public List<Booking> GetAllBookings()
    {
        return _bookings;
    }

    public Booking GetLastBooking()
    {
        if (_bookings.Count > 0)
            return _bookings.Last();
        else
            return null;
    }

    public void UpdateBooking(Booking updatedBooking)
    {
        var existingBooking = GetBookingById(updatedBooking.Id);
        if (existingBooking == null)
        {
            throw new KeyNotFoundException($"\nBooking ID {updatedBooking.Id} Not Found.");
        }
        existingBooking.Flight.Id = updatedBooking.Flight.Id;
        existingBooking.SeatClass = updatedBooking.SeatClass;
        SaveBookings();
    }

    private void LoadBookings()
    {
        var lines = _fileStorage.ReadAllLines(_filePath);
        foreach (var line in lines.Skip(1))
        {
            var parts = line.Split(',');
            if (parts.Length == 7)
            {
                try
                {
                    var _passenger = new Passenger
                    {
                        Id = int.Parse(parts[2]),
                        Name = parts[3],
                        Email = parts[4]
                    };

                    var flight = new Flight
                    {
                        Id = int.Parse(parts[1]),
                    };

                    var booking = new Booking
                    {
                        Id = int.Parse(parts[0]),
                        Flight = flight,
                        SeatClass = parts[5].ParseEnum(SeatClass.None),
                        Passenger = _passenger,
                        BookDate = DateTime.Parse(parts[6])
                    };
                        
                    _bookings.Add(booking);
                }
                catch (Exception ex)
                {
                    throw new ApplicationException($"\nError loading bookings: {ex.Message}", ex);
                }
            }
        }
    }

    public List<(Booking Booking, Flight Flight)> FilteredBookings(
List<Flight> filteredFlights, double? price, SeatClass seatClass, string passenger)
    {
        var filteredBookings = (from b in _bookings
                                join f in filteredFlights on b.Flight.Id equals f.Id
                                where (price == null || _flightsService.GetPriceByClass(f, seatClass) <= price.Value) &&
                                      (string.IsNullOrEmpty(passenger) || b.Passenger.Name.Equals(passenger, StringComparison.OrdinalIgnoreCase)) &&
                                      (seatClass == null || b.SeatClass == seatClass)
                                select (Booking: b, Flight: f))
                                 .ToList();

        return filteredBookings;
    }

    private void SaveBookings()
    {
        var lines = new List<string>
        {
            "Id,FlightId,PassengerId,PassengerName,PassengerEmail,Class,BookDate"
        };

        lines.AddRange(_bookings.Select(b =>
            $"{b.Id},{b.Flight.Id},{b.Passenger.Id},{b.Passenger.Name},{b.Passenger.Email},{b.SeatClass},{b.BookDate}"
        ));

        _fileStorage.WriteAllLines(_filePath, lines);
    }
}