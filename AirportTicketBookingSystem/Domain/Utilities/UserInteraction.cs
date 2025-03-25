using AirportTicketBookingSystem.Domain.Utilities;

namespace AirportTicketBookingSystem.Utilities;

public class UserInteraction
{
    private readonly MainMenuHandler _mainMenuHandler;

    public UserInteraction
        (BookingHandler bookingHandler, FlightSearchHandler flightSearchHandler, AdminHandler adminHandler)
    {
        _mainMenuHandler = new MainMenuHandler(bookingHandler, flightSearchHandler, adminHandler);
    }

    public void Start()
    {
        _mainMenuHandler.DisplayMainMenu();
    }
}
