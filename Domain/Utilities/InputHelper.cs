using System;
using AirportTicketBookingSystem.Domain.Models;
using AirportTicketBookingSystem.Domain.Utilities;

namespace AirportTicketBookingSystem.Utilities
{
    public static class InputHelper
    {
        public static int? GetIntegerInput(string message)
        {
            Console.WriteLine(message);
            if (int.TryParse(Console.ReadLine(), out int result))
                return result;
            return null;
        }

        public static Passenger GetPassengerInfo(int id)
        {
            Console.WriteLine("Enter your name:");
            string name = Console.ReadLine();

            Console.WriteLine("Enter your email:");
            string email = Console.ReadLine();

            return new Passenger
            {
                Id = id,
                Name = name,
                Email = email,
            };
        }

        public static SeatClass ValidateSeatClass()
        {
            while (true)
            {
                Console.Write("Class (Economy, Business, FirstClass): ");
                string input = Console.ReadLine().Trim();

                SeatClass seatClass = input.ParseEnum(SeatClass.None);
                if (seatClass != SeatClass.None)
                {
                    return seatClass;
                }

                Console.WriteLine("Invalid class. Please enter 'Economy', 'Business', or 'FirstClass'.");
            }
        }

    }
}
