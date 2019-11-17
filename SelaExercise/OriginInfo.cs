using System.Collections.Generic;
using System.Linq;

namespace SelaExercise
{
    /// <summary>
    /// Contains all necessary information about the flights from a specific origin
    /// </summary>
    public class OriginInfo
    {
        public readonly City Origin;

        public readonly IDictionary<string, OriginToDestinationInfo> Destinations;

        private Airline airlineWithMostFlights;
        public Airline AirlineWithMostFlights 
        { 
            get 
            {
                if (!isAirlineWithMostFlightsUpdated)
                {
                    var airlineWithMostFlights = GetAirlineWithMostFlights();
                    this.airlineWithMostFlights = airlineWithMostFlights.Item1;
                    AirlineWithMostFlightsCount = airlineWithMostFlights.Item2;
                }
                return airlineWithMostFlights;
            }

            private set { airlineWithMostFlights = value; } 
        }

        public int AirlineWithMostFlightsCount { get; private set; }

        private bool isAirlineWithMostFlightsUpdated;

        private readonly SortedDictionary<int, HashSet<City>> farthestDestinations;

        public OriginInfo(City origin)
        {
            Origin = origin;
            Destinations = new Dictionary<string, OriginToDestinationInfo>();
            farthestDestinations = new SortedDictionary<int, HashSet<City>>();
        }

        public void AddFlight(Flight flight)
        {
            if (!Destinations.ContainsKey(flight.Destination.Name))
                Destinations.Add(flight.Destination.Name, new OriginToDestinationInfo(Origin, flight.Destination));
            Destinations[flight.Destination.Name].AddFlight(flight);
            isAirlineWithMostFlightsUpdated = false;
            UpdateFarthestDestinations(flight.Destination, flight.Distance);
        }

        private (Airline, int) GetAirlineWithMostFlights()
        {
            var counters = new Dictionary<Airline, int>();
            Airline maxCounterAirline = null;
            var maxCounterCount = 0;
            Airline currentAirline;
            foreach (var dest in Destinations.Values)
                foreach (var flight in dest.Flights)
                {
                    currentAirline = flight.Airline;
                    if (!counters.ContainsKey(currentAirline))
                        counters.Add(currentAirline, 1);
                    else
                        counters[currentAirline]++;
                    if (counters[currentAirline] > maxCounterCount)
                    {
                        maxCounterAirline = currentAirline;
                        maxCounterCount = counters[currentAirline];
                    }
                }
            return (maxCounterAirline, maxCounterCount);
        }

        private void UpdateFarthestDestinations(City dest, int distance)
        {
            var smallestKey = farthestDestinations.Count == 0 ? 0 : farthestDestinations.First().Key;
            if (distance >= smallestKey)
                if (farthestDestinations.ContainsKey(distance))
                    farthestDestinations[distance].Add(dest);
                else
                    farthestDestinations.Add(distance, new HashSet<City> { dest });
            if (farthestDestinations.Count > Tools.NUMBER_OF_FARTHEST_DESTINATIONS)
                farthestDestinations.Remove(smallestKey);
        }
        public IList<(City, int)> GetFarthestDestinations()
        {
            var destinations = new List<(City, int)>();
            var counter = 0;
            int i;
            KeyValuePair<int, HashSet<City>> keyValue;
            for (i = farthestDestinations.Count - 1; i >= 0; i--)
            {
                keyValue = farthestDestinations.ElementAt(i);
                foreach (var dest in keyValue.Value)
                {
                    destinations.Add((dest, keyValue.Key));
                    counter++;
                    if (counter == Tools.NUMBER_OF_FARTHEST_DESTINATIONS)
                        break;
                }
                if (counter == Tools.NUMBER_OF_FARTHEST_DESTINATIONS)
                    break;
            }
            return destinations;
        }

        public override string ToString()
        {
            return Origin.ToString();
        }
    }
}
