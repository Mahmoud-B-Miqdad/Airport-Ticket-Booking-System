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
    public void AddFlight_WhenCalled_ShouldInvokeAddFlightOnRepository()
    {
        var flight = _fixture.Create<Flight>();

        _flightService.AddFlight(flight);

        _mockFlightRepo.Verify(repo => repo.AddFlight(flight), Times.Once);
    }

    [Fact]
    public void GetAllFlights_WhenCalled_ShouldReturnAllFlights()
    {
        var flights = _fixture.CreateMany<Flight>(5).ToList();
        _mockFlightRepo.Setup(repo => repo.GetAllFlights()).Returns(flights);

        var result = _flightService.GetAllFlights();

        result.Should().BeEquivalentTo(flights);
    }

     [Fact]
    public void GetAllFlights_WhenNoFlightsAvailable_ShouldReturnEmptyList()
    {
        _mockFlightRepo.Setup(repo => repo.GetAllFlights()).Returns(new List<Flight>());

        var result = _flightService.GetAllFlights();

        result.Should().BeEmpty();
    }

    [Fact]
    public void GetFlightById_WhenFlightExists_ShouldReturnFlight()
    {
        var flight = _fixture.Create<Flight>();
        _mockFlightRepo.Setup(repo => repo.GetFlightById(flight.Id)).Returns(flight);

        var result = _flightService.GetFlightById(flight.Id);

        result.Should().BeEquivalentTo(flight);
    }

    [Fact]
    public void GetFlightById_WhenFlightDoesNotExist_ShouldReturnNull()
    {
        int invalidId = _fixture.Create<int>();
        _mockFlightRepo.Setup(repo => repo.GetFlightById(invalidId)).Returns((Flight?)null);

        var result = _flightService.GetFlightById(invalidId);

        result.Should().BeNull();
    }

    [Fact]
    public void SearchFlights_ShouldCallRepositorySearchFlights_WithCorrectParameters()
    {
        string departureCountry = "USA";
        string destinationCountry = "UK";
        string departureAirport = "JFK";
        string arrivalAirport = "LHR";
        DateTime? departureDate = new DateTime(2025, 5, 10);
        SeatClass? seatClass = SeatClass.Business;
        double? maxPrice = 500.0;

        _flightService.SearchFlights(departureCountry, destinationCountry, departureAirport,
                                     arrivalAirport, departureDate, seatClass, maxPrice);

        _mockFlightRepo.Verify(repo => repo.SearchFlights(departureCountry, destinationCountry,
            departureAirport, arrivalAirport, departureDate, seatClass, maxPrice), Times.Once);
    }


[Fact]
    public void GetPriceByClass_WhenSeatClassExists_ShouldReturnCorrectPrice()
    {
        var flight = _fixture.Create<Flight>();
        flight.Prices["business"] = 500.0;

        var price = flight.GetPriceByClass(SeatClass.Business);

        price.Should().Be(500.0);
    }

    [Fact]
    public void GetPriceByClass_WhenSeatClassDoesNotExist_ShouldReturnDefaultPrice()
    {
        var flight = _fixture.Create<Flight>();
        //flight.Prices["economy"] = 200.0;

        var price = flight.GetPriceByClass(SeatClass.Business);

        price.Should().Be(0.0);
        //price.Should().Be(200.0);
    }
}