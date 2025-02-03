using AirportTicketBookingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace AirportTicketBookingSystem.Domain.Services
{
    public class CSVFlightImporter
    {
        private readonly string _filePath;
        public List<string> Errors { get; private set; }

        public CSVFlightImporter()
        {
            string relativePath = @"Data\flights.csv";
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);

            Errors = new List<string>();
        }

        public List<Flight> ImportFlights()
        {
            var flights = new List<Flight>();
            Errors.Clear();

            if (!File.Exists(_filePath))
            {
                Errors.Add("Flight data file not found in Resources folder.");
                return flights;
            }

            try
            {
                var lines = File.ReadAllLines(_filePath);
                for (int i = 1; i < lines.Length; i++) 
                {
                    var parts = lines[i].Split(',');

                    if (parts.Length != 7)
                    {
                        Errors.Add($"Invalid data at line {i + 1}: Incorrect number of columns.");
                        continue;
                    }

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

                            /*this commint to be sure that List of error is work
                            DepartureDate = DateTime.ParseExact(parts[3], "yyyy-MM-dd", CultureInfo.InvariantCulture),*/

                            DepartureAirport = parts[4],
                            ArrivalAirport = parts[5],
                            Prices = prices
                        };

                        flights.Add(flight);
                    }
                    catch (Exception ex)
                    {
                        Errors.Add($"Invalid data at line {i + 1}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Errors.Add($"Failed to read file: {ex.Message}");
            }

            return flights;
        }
    }
}