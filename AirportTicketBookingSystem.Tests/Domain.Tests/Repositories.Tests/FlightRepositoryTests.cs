using AirportTicketBookingSystem.Domain.Models;
using AirportTicketBookingSystem.Domain.Repositories;
using AirportTicketBookingSystem.Domain.Utilities;
using AutoFixture;
using FluentAssertions;

public class FlightRepositoryTests
{
    private readonly FakeFileHandler _fileHandler;
    private readonly Fixture _fixture;
    private readonly FlightRepository _flightRepository;

    public FlightRepositoryTests()
    {
        _fileHandler = new FakeFileHandler();
        _fixture = new Fixture();

        _flightRepository = new FlightRepository(_fileHandler);
    }

    [Fact]
    public void AddFlight_ShouldAddFlightSuccessfully()
    {
        var flight = _fixture.Create<Flight>();

        _flightRepository.AddFlight(flight);
        var flights = _flightRepository.GetAllFlights();

        flights.Should().Contain(flight);
    }

    [Fact]
    public void AddFlight_Should_ThrowException_When_FlightAlreadyExists()
    {
        var flight = _fixture.Create<Flight>();

        _flightRepository.AddFlight(flight);
        Action act = () => _flightRepository.AddFlight(flight);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage($"\nBooking {flight.Id} already exists");
    }

    [Fact]
    public void GetAllFlights_Should_Return_AllFlights()
    {
        var flight1 = _fixture.Create<Flight>();
        var flight2 = _fixture.Create<Flight>();

        _flightRepository.AddFlight(flight1);
        _flightRepository.AddFlight(flight2);

        var result = _flightRepository.GetAllFlights();

        result.Should().HaveCount(2).And.ContainEquivalentOf(flight1).And.ContainEquivalentOf(flight2);
    }

    [Fact]
    public void GetAllFlights_ShouldReturnEmptyList_WhenNoFlightsAdded()
    {
        var flights = _flightRepository.GetAllFlights();

        flights.Should().BeEmpty();
    }

    [Fact]
    public void GetFlightById_ShouldReturnFlight_WhenFlightExists()
    {
        var flight = _fixture.Create<Flight>();
        _flightRepository.AddFlight(flight);

        var retrievedFlight = _flightRepository.GetFlightById(flight.Id);

        retrievedFlight.Should().NotBeNull();
        retrievedFlight.Id.Should().Be(flight.Id);
    }

    [Fact]
    public void GetFlightById_ShouldReturnNull_WhenFlightDoesNotExist()
    {
        var retrievedFlight = _flightRepository.GetFlightById(999);

        retrievedFlight.Should().BeNull();
    }
}
