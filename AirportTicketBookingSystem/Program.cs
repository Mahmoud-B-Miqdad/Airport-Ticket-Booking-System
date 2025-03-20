using AirportTicketBookingSystem.Domain.Repositories;
using AirportTicketBookingSystem.Domain.Services;
using AirportTicketBookingSystem.Domain.Utilities;
using AirportTicketBookingSystem.Utilities;

IFileHandler fileStorage = new FileHandler();
IFlightRepository flightRepository = new FlightRepository(fileStorage);
IFlightService flightService = new FlightService(flightRepository);
IBookingRepository bookingRepository = new BookingRepository(fileStorage, flightService);
IBookingService bookingService = new BookingService(bookingRepository, flightService);

var bookingHandler = new BookingHandler(bookingService, flightService);
var flightSearchHandler = new FlightSearchHandler(flightService);
var adminHandler = new AdminHandler(bookingService, flightService);

var userInteraction = new UserInteraction(bookingHandler, flightSearchHandler, adminHandler);
userInteraction.Start();