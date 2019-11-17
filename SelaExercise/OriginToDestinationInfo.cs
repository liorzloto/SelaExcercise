using System.Collections.Generic;

namespace SelaExercise
{
    /// <summary>
    /// Contains all necessary information about the flights from a specific origin to a specific destination
    /// </summary>
    public class OriginToDestinationInfo
    {
        public readonly City Origin;
        public readonly City Destination;
        public readonly IList<Flight> Flights;
        public decimal AvgDepartureDelay { get; private set; }

        private int numberOfFlightsWithDepartureDelayInfo;

        public decimal AvgArrivalDelay { get; private set; }

        private int numberOfFlightsWithArrivalDelayInfo;

        public OriginToDestinationInfo(City origin, City destination)
        {
            Origin = origin;
            Destination = destination;
            Flights = new List<Flight>();
        }

        public void AddFlight(Flight flight)
        {
            Flights.Add(flight);
            if (flight.DepartureDelay.HasValue)
                AvgDepartureDelay = CalculateAvgDelay(
                    flight.DepartureDelay.Value, AvgDepartureDelay, ++numberOfFlightsWithDepartureDelayInfo);
            if (flight.ArrivalDelay.HasValue)
                AvgArrivalDelay = CalculateAvgDelay(
                    flight.ArrivalDelay.Value, AvgArrivalDelay, ++numberOfFlightsWithArrivalDelayInfo);
        }

        private decimal CalculateAvgDelay(int delay, decimal currentAvg, int numOfFlights)
        {
            var ratio = decimal.Divide(numOfFlights - 1, numOfFlights);
            return currentAvg * ratio + delay * (1 - ratio);
        }

        public override string ToString()
        {
            return string.Format("{0} -> {1}", Origin, Destination);
        }
    }
}
