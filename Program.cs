using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AirportTicketBookingSystem.Utilities;
namespace AirportTicketBookingSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            var userInteraction = new UserInteraction();
            userInteraction.Start();
        }
    }
}