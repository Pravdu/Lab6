using Lab6;

class Program
{
    static void Main()
    {
        var flightSystem = new FlightInformationSystem();
        flightSystem.LoadFlightsFromJson("flights.json");

        var newFlight = new Flight
        {
            FlightNumber = "123",
            Airline = "AmericanAirlines",
            Destination = "New York",
            DepartureTime = DateTime.Now.AddHours(2),
            ArrivalTime = DateTime.Now.AddHours(4),
            Status = FlightStatus.OnTime,
            Duration = TimeSpan.FromHours(2),
            AircraftType = "F16",
            Terminal = "1"
        };

        flightSystem.AddFlight(newFlight);

        flightSystem.RemoveFlight("BA560");

        bool continueWorking = true;

        while (continueWorking)
        {

            Console.WriteLine("Enter an option (1-5):");
            Console.WriteLine("1 - Flights by airline");
            Console.WriteLine("2 - Delayed flights");
            Console.WriteLine("3 - Flights on a specific date");
            Console.WriteLine("4 - Flights in a specific time range to a specific destination");
            Console.WriteLine("5 - Recent arrivals");

            if (int.TryParse(Console.ReadLine(), out int option))
            {
                switch (option)
                {
                    case 1:
                        Console.WriteLine("Flights by airline:");
                        var airlineFlights = flightSystem.SearchFlightByAirLine("MAU");
                        PrintFlights(airlineFlights);
                        break;

                    case 2:
                        Console.WriteLine("Delayed flights:");
                        var delayedFlights = flightSystem.SearchDelayedFlights();
                        PrintFlights(delayedFlights);
                        break;

                    case 3:
                        DateTime specificDate = new DateTime(2023, 1, 1);
                        Console.WriteLine("Flights on a specific date:");
                        var specificFlights = flightSystem.SearchFlightByDepartureDate(specificDate.Date);
                        PrintFlights(specificFlights);
                        break;

                    case 4:
                        DateTime specificDates;
                        specificDates = new DateTime(2023, 3, 7);

                        DateTime startTime = new DateTime(specificDates.Year, specificDates.Month, specificDates.Day, 6, 0, 0);
                        DateTime endTime = new DateTime(specificDates.Year, specificDates.Month, specificDates.Day, 7, 0, 0);

                        Console.WriteLine("Flights in a specific time range to a specific destination:");
                        var timeAndDestinationFlights = flightSystem.SearchFlightByTimeAndDestination(startTime, endTime, "Odessa");
                        PrintFlights(timeAndDestinationFlights);
                        break;

                    case 5:
                        Console.WriteLine("Recent arrivals:");
                        DateTime specificDatess = new DateTime(2023, 3, 7);
                        DateTime startTimes = new DateTime(specificDatess.Year, specificDatess.Month, specificDatess.Day, 6, 0, 0);
                        DateTime endTimes = new DateTime(specificDatess.Year, specificDatess.Month, specificDatess.Day, 7, 0, 0);

                        Console.WriteLine("Flights in a specific time range:");
                        var timeRangeFlights = flightSystem.SearchRecentArrivals(startTimes, endTimes);
                        PrintFlights(timeRangeFlights);
                        break;

                    default:
                        Console.WriteLine("Invalid option");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a number");
                var serializedData = flightSystem.SerealizeFlightToJson();
                File.WriteAllText("flights_updated.json", serializedData);
            }

            static void PrintFlights(List<Flight> flights)
            {
                int visibleRows = Console.WindowHeight - 1;
                int startIndex = 0;

                while (startIndex < flights.Count)
                {
                    int endIndex = Math.Min(startIndex + visibleRows, flights.Count);
                    for (int i = startIndex; i < endIndex; i++)
                    {
                        Console.WriteLine($"{flights[i].FlightNumber} - {flights[i].Airline} - {flights[i].Destination} - {flights[i].DepartureTime} - {flights[i].ArrivalTime} - {flights[i].Status}");
                    }
                    startIndex = endIndex;

                    if (endIndex < flights.Count)
                    {
                        Console.WriteLine("Press Enter for more...");
                        Console.ReadLine();
                        Console.Clear();
                    }
                }
            }
            Console.WriteLine("Want to continue? (yes or no)");
            string answer = Console.ReadLine().ToLower();
            continueWorking = answer == "yes";
            if (answer == "no") ;
        }
        {
            continueWorking = false;
            Console.WriteLine("Program was over.");
        }
    }
}