using AutoFixture;
using FluentAssertions;
using Moq;
using AirportTicketBookingSystem.Domain.Models;
using AirportTicketBookingSystem.Domain.Repositories;
using AirportTicketBookingSystem.Domain.Services;

public class BookingServiceTests
{
    private readonly Mock<IBookingRepository> _mockBookingRepo;
    private readonly Mock<IFlightService> _mockFlightService;
    private readonly Fixture _fixture;
    private readonly BookingService _bookingService;

    public BookingServiceTests()
    {
        _mockBookingRepo = new Mock<IBookingRepository>();
        _mockFlightService = new Mock<IFlightService>();
        _fixture = new Fixture();
        _bookingService = new BookingService(_mockBookingRepo.Object, _mockFlightService.Object);
    }

    [Fact]
    public void BookFlight_WhenValidDataProvided_ShouldAddBooking()
    {
        var passenger = _fixture.Create<Passenger>();
        var seatClass = _fixture.Create<SeatClass>();
        int flightId = _fixture.Create<int>();

        _bookingService.BookFlight(flightId, passenger, seatClass);

        _mockBookingRepo.Verify(repo => repo.AddBooking(It.IsAny<Booking>()), Times.Once);
    }

    [Fact]
    public void BookFlight_WhenFlightIdIsNull_ShouldThrowException()
    {
        var passenger = _fixture.Create<Passenger>();
        var seatClass = _fixture.Create<SeatClass>();

        Action act = () => _bookingService.BookFlight(null, passenger, seatClass);

        act.Should().Throw<ArgumentNullException>();
    }


    [Fact]
    public void GetBookingsByPassenger_WhenPassengerHasBookings_ShouldReturnBookings()
    {
        int passengerId = _fixture.Create<int>();
        var bookings = _fixture.Create<List<Booking>>();
        _mockBookingRepo.Setup(repo => repo.GetBookingsByPassenger(passengerId)).Returns(bookings);

        var result = _bookingService.GetBookingsByPassenger(passengerId);

        result.Should().BeEquivalentTo(bookings);
    }

    [Fact]
    public void GetBookingsByPassenger_WhenPassengerHasNoBookings_ShouldReturnEmptyList()
    {
        int passengerId = _fixture.Create<int>();
        _mockBookingRepo.Setup(repo => repo.GetBookingsByPassenger(passengerId))
            .Returns(new List<Booking>());

        var result = _bookingService.GetBookingsByPassenger(passengerId);

        result.Should().BeEmpty();
    }

    [Fact]
    public void CancelBooking_WhenBookingIdIsNotNull_ShouldCallRepositoryCancel()
    {
        int bookingId = _fixture.Create<int>();

        _bookingService.CancelBooking(bookingId);

        _mockBookingRepo.Verify(repo => repo.CancelBooking(bookingId), Times.Once);
    }

    [Fact]
    public void GetAllBookings_WhenCalled_ShouldReturnAllBookings()
    {
        var bookings = _fixture.Create<List<Booking>>();
        _mockBookingRepo.Setup(repo => repo.GetAllBookings()).Returns(bookings);

        var result = _bookingService.GetAllBookings();

        result.Should().BeEquivalentTo(bookings);
    }

    [Fact]
    public void GetAllBookings_WhenNoBookingsExist_ShouldReturnEmptyList()
    {
        var bookings = _fixture.Create<List<Booking>>();
        _mockBookingRepo.Setup(repo => repo.GetAllBookings()).Returns(new List<Booking>());

        var result = _bookingService.GetAllBookings();

        result.Should().BeEmpty();
    }

    [Fact]
    public void GetLastBooking_WhenBookingsExist_ShouldReturnLastBooking()
    {
        var booking = _fixture.Create<Booking>();
        _mockBookingRepo.Setup(repo => repo.GetLastBooking()).Returns(booking);

        var result = _bookingService.GetLastBooking();

        result.Should().Be(booking);
    }

    [Fact]
    public void GetLastBooking_WhenNoBookingsExist_ShouldReturnNull()
    {
        _mockBookingRepo.Setup(repo => repo.GetLastBooking()).Returns((Booking?)null);

        var result = _bookingService.GetLastBooking();

        result.Should().BeNull();
    }

    [Fact]
    public void ModifyBooking_WhenBookingExists_ShouldUpdateBooking()
    {
        int bookingId = _fixture.Create<int>();
        int newFlightId = _fixture.Create<int>();
        var newSeatClass = _fixture.Create<SeatClass>();
        var booking = _fixture.Create<Booking>();
        _mockBookingRepo.Setup(repo => repo.GetBookingById(bookingId)).Returns(booking);

        _bookingService.ModifyBooking(bookingId, newFlightId, newSeatClass);

        booking.Flight.Id.Should().Be(newFlightId);
        booking.SeatClass.Should().Be(newSeatClass);
        _mockBookingRepo.Verify(repo => repo.UpdateBooking(booking), Times.Once);
    }

    [Fact]
    public void ModifyBooking_WhenBookingNotFound_ShouldThrowException()
    {
        int bookingId = _fixture.Create<int>();
        int newFlightId = _fixture.Create<int>();
        var newSeatClass = _fixture.Create<SeatClass>();
        _mockBookingRepo.Setup(repo => repo.GetBookingById(bookingId)).Returns((Booking?)null);

        Action act = () => _bookingService.ModifyBooking(bookingId, newFlightId, newSeatClass);

        act.Should().Throw<KeyNotFoundException>().WithMessage($"Booking with ID {bookingId} not found.");
    }
}