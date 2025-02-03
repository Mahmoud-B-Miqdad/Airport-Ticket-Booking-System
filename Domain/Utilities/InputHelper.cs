using System;
using AirportTicketBookingSystem.Domain.Models;

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

        public static string ValidateSeatClass()
        {
            string seatClass = "";
            while (true)
            {
                Console.Write("Class (Economy, Business, First Class): ");
                seatClass = Console.ReadLine().Trim();

                if (seatClass == "")
                {
                    Console.WriteLine("Invalid class. Please enter 'Economy', 'Business', or 'First Class'.");
                    continue;
                }

                if (string.IsNullOrEmpty(seatClass) ||
                    seatClass.Equals("Economy", StringComparison.OrdinalIgnoreCase) ||
                    seatClass.Equals("Business", StringComparison.OrdinalIgnoreCase) ||
                    seatClass.Equals("First Class", StringComparison.OrdinalIgnoreCase))
                {
                    return seatClass;
                }
                Console.WriteLine("Invalid class. Please enter 'Economy', 'Business', or 'First Class'.");
            }

        }
    }
}
