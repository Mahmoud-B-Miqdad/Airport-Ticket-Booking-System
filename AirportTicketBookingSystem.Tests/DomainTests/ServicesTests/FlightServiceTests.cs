using Moq;
using FluentAssertions;
using AutoFixture;
using AirportTicketBookingSystem.Domain.Models;
using AirportTicketBookingSystem.Domain.Repositories;
using AirportTicketBookingSystem.Domain.Services;
public class FlightServiceTests
{
    private readonly Mock<IFlightRepository> _mockFlightRepo;
    private readonly FlightService _flightService;
    private readonly Fixture _fixture;

    public FlightServiceTests()
    {
        _mockFlightRepo = new Mock<IFlightRepository>();
        _flightService = new FlightService(_mockFlightRepo.Object);
        _fixture = new Fixture();
    }

    [Fact]
    public void AddFlight_ShouldCallRepositoryAddFlight()
    {
        var flight = _fixture.Create<Flight>();

        _flightService.AddFlight(flight);

        _mockFlightRepo.Verify(repo => repo.AddFlight(flight), Times.Once);
    }

    [Fact]
    public void AddFlight_ShouldThrowException_WhenFlightAlreadyExists()
    {
        var flight = _fixture.Create<Flight>();
        _mockFlightRepo.Setup(repo => repo.AddFlight(It.IsAny<Flight>()))
            .Throws(new InvalidOperationException($"\nBooking {flight.Id} already exists"));

        Action act = () => _flightService.AddFlight(flight);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage($"\nBooking {flight.Id} already exists");
    }

    [Fact]
    public void GetAllFlights_ShouldReturnAllFlights()
    {
        var flights = _fixture.CreateMany<Flight>(5).ToList();
        _mockFlightRepo.Setup(repo => repo.GetAllFlights()).Returns(flights);

        var result = _flightService.GetAllFlights();

        result.Should().BeEquivalentTo(flights);
    }

     [Fact]
    public void GetAllFlights_ShouldReturnEmptyList_WhenNoFlightsAvailable()
    {
        _mockFlightRepo.Setup(repo => repo.GetAllFlights()).Returns(new List<Flight>());

        var result = _flightService.GetAllFlights();

        result.Should().BeEmpty();
    }

    [Fact]
    public void GetFlightById_ShouldReturnFlight_WhenFlightExists()
    {
        var flight = _fixture.Create<Flight>();
        _mockFlightRepo.Setup(repo => repo.GetFlightById(flight.Id)).Returns(flight);

        var result = _flightService.GetFlightById(flight.Id);

        result.Should().BeEquivalentTo(flight);
    }

    [Fact]
    public void GetFlightById_ShouldReturnNull_WhenFlightDoesNotExist()
    {
        int invalidId = _fixture.Create<int>();
        _mockFlightRepo.Setup(repo => repo.GetFlightById(invalidId)).Returns((Flight?)null);

        var result = _flightService.GetFlightById(invalidId);

        result.Should().BeNull();
    }

    [Fact]
    public void SearchFlights_ShouldReturnMatchingFlights()
    {
        var flights = _fixture.CreateMany<Flight>(10).ToList();
        var targetFlight = flights.First();
        targetFlight.DepartureCountry = "USA";
        _mockFlightRepo.Setup(repo => repo.GetAllFlights()).Returns(flights);

        var result = _flightService.SearchFlights(departureCountry: "USA");

        result.Should().ContainSingle(f => f.DepartureCountry == "USA");
    }

    [Fact]
    public void SearchFlights_ShouldReturnEmptyList_WhenNoMatch()
    {
        var flights = _fixture.CreateMany<Flight>(5).ToList();
        _mockFlightRepo.Setup(repo => repo.GetAllFlights()).Returns(flights);

        var result = _flightService.SearchFlights(departureCountry: "Mars");

        result.Should().BeEmpty();
    }

    [Fact]
    public void GetPriceByClass_ShouldReturnCorrectPrice_WhenSeatClassExists()
    {
        var flight = _fixture.Create<Flight>();
        flight.Prices["business"] = 500.0;

        var price = _flightService.GetPriceByClass(flight, SeatClass.Business);

        price.Should().Be(500.0);
    }

    [Fact]
    public void GetPriceByClass_ShouldReturnDefaultPrice_WhenSeatClassDoesNotExist()
    {
        var flight = _fixture.Create<Flight>();
        //flight.Prices["economy"] = 200.0;

        var price = _flightService.GetPriceByClass(flight, SeatClass.Business);

        price.Should().Be(0.0);
        //price.Should().Be(200.0);
    }
}