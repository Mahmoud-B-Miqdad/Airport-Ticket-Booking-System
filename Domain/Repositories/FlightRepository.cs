using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AirportTicketBookingSystem.Domain.Models;

namespace AirportTicketBookingSystem.Domain.Repositories
{
    public class FlightRepository
    {
        private readonly string _filePath;
        private readonly List<Flight> _flights;

        public FlightRepository()
        {
            string dataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            _filePath = Path.Combine(dataFolder, "flights.csv");

            if (!Directory.Exists(dataFolder))
            {
                Directory.CreateDirectory(dataFolder);
            }

            _flights = new List<Flight>();

            if (File.Exists(_filePath))
            {
                LoadFlights();
            }
        }

        public void AddFlight(Flight flight)
        {
            _flights.Add(flight);
            SaveFlights();
        }

        public List<Flight> GetAllFlights()
        {
            return _flights;
        }

        private void LoadFlights()
        {
            var lines = File.ReadAllLines(_filePath);
            foreach (var line in lines.Skip(1)) 
            {
                var parts = line.Split(',');
                if (parts.Length == 9)
                {
                    try
                    {
                        var flight = new Flight(
                            int.Parse(parts[0]),
                            parts[1], parts[2], parts[3], parts[4],
                            DateTime.Parse(parts[5]),
                            double.Parse(parts[6]), double.Parse(parts[7]), double.Parse(parts[8])
                        );
                        _flights.Add(flight);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"⚠️ Error in loading the trip: {ex.Message}");
                    }
                }
            }
        }

        private void SaveFlights()
        {
            var lines = new List<string>
            {
                "Id,DepartureCountry,DestinationCountry,DepartureAirport,ArrivalAirport,DepartureDate,EconomyPrice,BusinessPrice,FirstClassPrice"
            };

            lines.AddRange(_flights.Select(f =>
                $"{f.Id},{f.DepartureCountry},{f.DestinationCountry},{f.DepartureAirport},{f.ArrivalAirport},{f.DepartureDate:yyyy-MM-dd},{f.EconomyPrice},{f.BusinessPrice},{f.FirstClassPrice}"
            ));

            File.WriteAllLines(_filePath, lines);
        }
    }
}
