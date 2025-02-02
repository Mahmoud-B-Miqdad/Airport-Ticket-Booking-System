﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using AirportTicketBookingSystem.Domain.Models;

namespace AirportTicketBookingSystem.Domain.Repositories
{
    public class FlightRepository
    {
        private readonly string _filePath;
        private readonly List<Flight> _flights;

        public FlightRepository()
        {

            string relativePath = @"Data\flights.csv";
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);

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

        public Flight GetFlightById(int? flightId)
        {
            return _flights.FirstOrDefault(f => f.Id == flightId);
        }

        private void LoadFlights()
        {
            var lines = File.ReadAllLines(_filePath);
            foreach (var line in lines.Skip(1)) 
            {
                var parts = line.Split(',');
                if (parts.Length == 7) 
                {
                    try
                    {
                        var prices = parts[6]
                            .Split('|')
                            .Select(p => p.Split(':'))
                            .ToDictionary(p => p[0], p => double.Parse(p[1]));

                        var flight = new Flight
                        {
                            Id = int.Parse(parts[0]),
                            DepartureCountry = parts[1],
                            DestinationCountry = parts[2],
                            DepartureDate = DateTime.Parse(parts[3]),
                            DepartureAirport = parts[4],
                            ArrivalAirport = parts[5],
                            Prices = prices
                        };

                        _flights.Add(flight);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error in loading the trip: {ex.Message}");
                    }
                }
            }
        }


        private void SaveFlights()
        {
            var lines = new List<string>
    {
        "Id,DepartureCountry,DestinationCountry,DepartureDate,DepartureAirport,ArrivalAirport,Prices"
    };

            lines.AddRange(_flights.Select(f =>
                $"{f.Id},{f.DepartureCountry},{f.DestinationCountry},{f.DepartureDate:yyyy-MM-dd},{f.DepartureAirport},{f.ArrivalAirport}," +
                $"{string.Join("|", f.Prices.Select(p => $"{p.Key}:{p.Value}"))}"
            ));

            File.WriteAllLines(_filePath, lines);
        }
    }
}