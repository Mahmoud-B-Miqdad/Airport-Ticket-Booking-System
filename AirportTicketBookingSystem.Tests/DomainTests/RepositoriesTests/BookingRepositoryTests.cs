using Moq;
using AutoFixture;
using FluentAssertions;
using Shouldly;
using AirportTicketBookingSystem.Domain.Models;
using AirportTicketBookingSystem.Domain.Repositories;
using AirportTicketBookingSystem.Domain.Services;
using AirportTicketBookingSystem.Domain.Utilities;
using AirportTicketBookingSystem.Domain.Massages;

public class BookingRepositoryTests : IDisposable
{
    private readonly Fixture _fixture;
    private readonly IFileHandler _fileStorage;
    private readonly Mock<IFlightService> _flightServiceMock;
    private readonly BookingRepository _bookingRepository;

    public BookingRepositoryTests()
    {
        _fixture = new Fixture();
        _fileStorage = new FakeFileHandler();
        _flightServiceMock = new Mock<IFlightService>();

        _bookingRepository = new BookingRepository(_fileStorage, _flightServiceMock.Object);
    }

    [Fact]
    public void AddBooking_WhenValidBookingProvided_ShouldAddBookingToList()
    {
        var booking = _fixture.Create<Booking>();
        //var booking = new Booking { Id = 1, Flight = new Flight { Id = 101 }, Passenger = new Passenger
        //{ Id = 1, Name = "Mahmoud Miqdad" }, SeatClass = SeatClass.Economy, BookDate = DateTime.Now };

        _bookingRepository.AddBooking(booking);

        _bookingRepository.GetAllBookings().ShouldContain(booking);
    }

    [Fact]
    public void AddBooking_WhenBookingAlreadyExists_ShouldThrowException()
    {
        var booking = _fixture.Create<Booking>();

        _bookingRepository.AddBooking(booking);
        Action act = () => _bookingRepository.AddBooking(booking);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage(string.Format(ErrorMessages.BookingAlreadyExists, booking.Id));
    }

    [Fact]
    public void GetBookingsByPassenger_WhenPassengerHasBookings_ShouldReturnCorrectBookings()
    {
        var booking = _fixture.Create<Booking>();

        _bookingRepository.AddBooking(booking);
        var result = _bookingRepository.GetBookingsByPassenger(booking.Passenger.Id);

        result.Should().ContainSingle().Which.Should().Be(booking);
    }

    [Fact]
    public void GetBookingsByPassenger_WhenNoBookingsForGivenPassenger_ShouldReturnEmptyList()
    {
        var passenger = _fixture.Build<Passenger>()
                       .With(p => p.Id, 2).Create();

        var booking = _fixture.Build<Booking>()
                             .With(b => b.Passenger, passenger).Create();

        _bookingRepository.AddBooking(booking);

        var result = _bookingRepository.GetBookingsByPassenger(5);
        result.Should().BeEmpty();
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void GetBookingsByPassenger_WhenPassengerIdIsInvalid_ShouldThrowException(int invalidPassengerId)
    {
        Action act = () => _bookingRepository.GetBookingsByPassenger(invalidPassengerId);
        act.Should().Throw<ArgumentException>().WithMessage(string.Format(ErrorMessages.InvalidPassengerID, invalidPassengerId));
    }

    [Fact]
    public void GetBookingById_WhenBookingExists_ShouldReturnCorrectBooking()
    {
        var booking1 = _fixture.Create<Booking>();
        var booking2 = _fixture.Create<Booking>();
        _bookingRepository.AddBooking(booking1);
        _bookingRepository.AddBooking(booking2);

        var result = _bookingRepository.GetBookingById(booking2.Id);

        result.ShouldNotBeNull();
        result.Id.ShouldBe(booking2.Id);
    }

    [Fact]
    public void GetBookingById_WhenBookingNotFound_ShouldReturnNull()
    {
        var result = _bookingRepository.GetBookingById(999);
        result.ShouldBeNull();
    }

    [Fact]
    public void CancelBooking_WhenBookingExists_ShouldRemoveBooking()
    {
        var booking1 = _fixture.Create<Booking>();
        var booking2 = _fixture.Create<Booking>();
        _bookingRepository.AddBooking(booking1);
        _bookingRepository.AddBooking(booking2);

        _bookingRepository.CancelBooking(booking1.Id);

        _bookingRepository.GetBookingById(booking1.Id).ShouldBeNull();
    }

    [Fact]
    public void CancelBooking_WhenBookingNotFound_ShouldThrowException()
    {
        Action act = () => _bookingRepository.CancelBooking(999);
        act.Should().Throw<KeyNotFoundException>().WithMessage(string.Format(ErrorMessages.BookingNotFound, 999));
    }

    [Fact]
    public void GetAllBookings_WhenCalled_ShouldReturnAllBookings()
    {
        var booking1 = _fixture.Create<Booking>();
        var booking2 = _fixture.Create<Booking>();
        _bookingRepository.AddBooking(booking1);
        _bookingRepository.AddBooking(booking2);


        var result = _bookingRepository.GetAllBookings();

        result.Should().HaveCount(2).And.Contain(booking1).And.Contain(booking2);
    }

    [Fact]
    public void GetAllBookings_WhenNoBookingsExist_ShouldReturnEmptyList()
    {
        var result = _bookingRepository.GetAllBookings();
        result.Should().BeEmpty();
    }

    [Fact]
    public void GetLastBooking_WhenBookingsExist_ShouldReturnLastBooking()
    {
        var booking1 = _fixture.Create<Booking>();
        var booking2 = _fixture.Create<Booking>();
        var booking3 = _fixture.Create<Booking>();
        _bookingRepository.AddBooking(booking1);
        _bookingRepository.AddBooking(booking2);
        _bookingRepository.AddBooking(booking3);


        var result = _bookingRepository.GetLastBooking();
        result.Should().Be(booking3);
    }

    [Fact]
    public void GetLastBooking_WhenNoBookingsExist_ShouldReturnNull()
    {
        var result = _bookingRepository.GetLastBooking();
        result.ShouldBeNull();
    }

    [Fact]
    public void UpdateBooking_WhenBookingExists_ShouldModifyExistingBooking()
    {
        var booking = _fixture.Build<Booking>()
                      .With(b => b.Id, 1).Create();

        var updatedFlight = _fixture.Build<Flight>()
       .With(f => f.Id, 7).Create();

        var updatedBooking = _fixture.Build<Booking>()
                      .With(b => b.Id, 1)
                      .With(b => b.SeatClass, SeatClass.FirstClass)
                      .With(b => b.Flight, updatedFlight).Create();

        _bookingRepository.AddBooking(booking);
        _bookingRepository.UpdateBooking(updatedBooking);

        var result = _bookingRepository.GetBookingById(booking.Id);

        result.Flight.Id.Should().Be(updatedBooking.Flight.Id);
        result.SeatClass.Should().Be(updatedBooking.SeatClass);
    }

    [Fact]
    public void UpdateBooking_WhenBookingNotFound_ShouldThrowException()
    {
        var updatedBooking = _fixture.Build<Booking>()
                     .With(b => b.Id, 999).Create();

        Action act = () => _bookingRepository.UpdateBooking(updatedBooking);

        act.Should().Throw<KeyNotFoundException>()
            .WithMessage(string.Format(ErrorMessages.BookingNotFound, updatedBooking.Id));
    }

    public void Dispose()
    {
        if (_fileStorage is FakeFileHandler fakeFileHandler)
        {
            fakeFileHandler.DeleteFile();
        }
    }
}