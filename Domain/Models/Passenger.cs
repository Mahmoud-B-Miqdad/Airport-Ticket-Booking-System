using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTicketBookingSystem.Domain.Models
{
    public class Passenger
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        //public Passenger(int id, string name, string email)
        //{
        //    Id = id;
        //    Name = name;
        //    Email = email;
        //}

        public override string ToString()
        {
            return $"{Id}: {Name} ({Email})";
        }
    }
}

