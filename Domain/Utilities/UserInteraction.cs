using AirportTicketBookingSystem.Domain.Models;
using AirportTicketBookingSystem.Domain.Services;
using System;

namespace AirportTicketBookingSystem.Utilities
{
    public class UserInteraction
    {
        private readonly MainMenuHandler _mainMenuHandler;

        public UserInteraction()
        {
            _mainMenuHandler = new MainMenuHandler();
        }

        public void Start()
        {
            _mainMenuHandler.DisplayMainMenu();
        }
    }
}
