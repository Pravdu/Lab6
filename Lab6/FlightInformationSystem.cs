using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Lab6
{
    public class FlightInformationSystem
    {
        private List<Flight> flights;
        public FlightInformationSystem() 
        { 
        flights = new List<Flight>();
        }
        public void AddFlight(Flight flight) 
        {
        flights.Add(flight);
        }
        public class FlightData
        {
            [JsonPropertyName("flights")]
            public List<Flight> Flights { get; set; }
        }
        public void RemoveFlight(string FlightNumber)
        {
            var flightToRemove = flights.FirstOrDefault( f => f.FlightNumber == FlightNumber);
            if (flightToRemove != null) 
                flights.Remove(flightToRemove);
        }
        public List<Flight> SearchFlightByAirLine(string Airline)
        {
            return flights.Where(f => f.Airline == Airline)
                .OrderBy(f => f.DepartureTime)
                .ToList();
        }
        public List<Flight> SearchDelayedFlights()
        {
            return flights.Where(f => f.Status == FlightStatus.Delayed)
                .OrderBy(f => f.DepartureTime)
                .ToList();
        }
        public List<Flight> SearchFlightByDepartureDate(DateTime departuredate)
        {
            return flights.Where(f => f.DepartureTime.Date == departuredate.Date)
                .OrderBy(f => f.DepartureTime)
                .ToList();
        }
        public List<Flight> SearchFlightByTimeAndDestination(DateTime starttime, DateTime endtime, string destination)
        {
            return flights.Where(f=> f.DepartureTime >=starttime && f.DepartureTime <= endtime && f.Destination == destination)
                .OrderBy(f => f.DepartureTime)
                .ToList();
        }
        public List<Flight> SearchRecentArrivals(DateTime endTime, DateTime endTimes)
        {
            var startTime = endTime.AddHours(-1);
            return flights.Where(f => f.ArrivalTime >= startTime && f.ArrivalTime <= endTime)
                .OrderBy(f => f.ArrivalTime)
                .ToList();
        }
        public void LoadFlightsFromJson(string filepath)
        {
            try
            {
                string jsonData = File.ReadAllText(filepath);
                var flightData = JsonConvert.DeserializeObject<FlightData>(jsonData);
                if (flightData !=null)
                {
                    foreach (var flight in flightData.Flights)
                    {
                        if (ValidateFlight(flight))
                        {
                            flights.Add(flight);
                        }
                        else 
                        {
                            Console.WriteLine("Invalid data flight found: {0}", flight);
                        }
                    }
                    Console.WriteLine("Flights loaded successfully");
                }
                else
                {
                    Console.WriteLine("Error: Not founded valid data flight in JSON");
                }  
            }
            catch(JsonSerializationException ex)
            {
                Console.WriteLine($"Error loading flights from JSON: {ex.Message}");
            }

        }
        private bool ValidateFlight(Flight flight)
        {
            if (flight.FlightNumber.Length != 5 &&  
                flight.FlightNumber.Length != 7) 
            {
                return false;
            }

            if (string.IsNullOrEmpty(flight.FlightNumber) || 
                string.IsNullOrEmpty(flight.Airline) ||
                string.IsNullOrEmpty(flight.Destination) ||
                string.IsNullOrEmpty(flight.FlightNumber) ||
                flight.DepartureTime == null ||
                flight.ArrivalTime == null)
            {
                return false;
            }

            if (flight.ArrivalTime <= flight.DepartureTime)
            {
                return false;
            }

            return true;
        }

        public string SerealizeFlightToJson()
        {
            try
            {
                return JsonConvert.SerializeObject(flights, Newtonsoft.Json.Formatting.Indented);
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error serializing flights to JSON: {ex.Message}");
                return null;    
            }
        }
    }
}
