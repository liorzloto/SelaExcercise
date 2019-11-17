using System;
using System.Collections.Generic;

namespace SelaExercise
{
    /// <summary>
    /// Contains all necessary information about the flights
    /// </summary>
    public partial class FlightsInfo
    {
        private readonly IDictionary<string, OriginInfo> origins;

        private readonly IDictionary<string, Airline> airlines;

        public FlightsInfo()
        {
            origins = new Dictionary<string, OriginInfo>();
            airlines = new Dictionary<string, Airline>();
        }

        // Creates a flights and adds it correctly according to the origin and the destination.
        // Creates each necessary object if needed
        public void AddFlight(string origin, string dest, string airline, 
            DateTime depDate, DateTime arrDate, int? depDelay, int? arrDelay, int distance)
        {
            if (!origins.ContainsKey(origin))
                origins.Add(origin, new OriginInfo(new City(origin)));
            if (!origins.ContainsKey(dest))
                origins.Add(dest, new OriginInfo(new City(dest)));
            if (!airlines.ContainsKey(airline))
                airlines.Add(airline, new Airline(airline));

            origins[origin].AddFlight(new Flight(origins[origin].Origin, origins[dest].Origin, airlines[airline],
                depDate, arrDate, depDelay, arrDelay, distance));
        }

        public Tuple<decimal, decimal, int> GetAvgDepartureAndArrivalDelays(string originA, string destB, out Result result)
        {
            if (!origins.ContainsKey(originA))
            {
                result = Result.CityANotExist;
                return null;
            }
            if (!origins.ContainsKey(destB))
            {
                result = Result.CityBNotExist;
                return null;
            }
            
            var origin = origins[originA];
            if (!origin.Destinations.ContainsKey(destB))
            {
                result = Result.NoFlightsFromAToB;
                return null;
            }

            result = Result.Success;
            var originToDestinationInfo = origin.Destinations[destB];
            return new Tuple<decimal, decimal, int>(originToDestinationInfo.AvgDepartureDelay, 
                originToDestinationInfo.AvgArrivalDelay, originToDestinationInfo.Flights.Count);
        }

        public Tuple<Airline,int> GetAirlineWithMostFlights(string originA, out Result result)
        {
            if (!origins.ContainsKey(originA))
            {
                result = Result.CityANotExist;
                return null;
            }
            var origin = origins[originA];
            var airline = origin.AirlineWithMostFlights;
            if (airline == null)
            {
                result = Result.NoFlightsFromCityA;
                return null;
            }
            result = Result.Success;
            return new Tuple<Airline, int>(airline, origin.AirlineWithMostFlightsCount);
        }

        public IList<(City, int)> GetFurthestDestinations(string originA, out Result result)
        {
            if (!origins.ContainsKey(originA))
            {
                result = Result.CityANotExist;
                return null;
            }
            var destinations = origins[originA].GetFarthestDestinations();
            result = destinations.Count == 0 ? Result.NoFlightsFromCityA : Result.Success;
            return destinations;
        }

        public Tuple<City, decimal> GetJourneyWithMinimalAvgArrivalDelay(string originA, string destB, out Result result)
        {
            if (!origins.ContainsKey(originA))
            {
                result = Result.CityANotExist;
                return null;
            }
            if (!origins.ContainsKey(destB))
            {
                result = Result.CityBNotExist;
                return null;
            }

            var minAvgArrDelay = decimal.MaxValue;
            City minCity = null;
            decimal calculatedAvgDelay;

            // Foreach city C which is fliable from city A, calculate the arrival delay to city B through city C
            foreach (var aToCInfo in origins[originA].Destinations)
            {
                // Cannot fly to city B through city B or through city without flights to B
                if (aToCInfo.Value.Destination.Name == destB || 
                    !origins[aToCInfo.Value.Destination.Name].Destinations.ContainsKey(destB))
                    continue;

                calculatedAvgDelay = GetAvgArrivalDelayThroughCityC(aToCInfo.Value, destB);
                if (calculatedAvgDelay >= 0 && calculatedAvgDelay <= minAvgArrDelay)
                {
                    minAvgArrDelay = calculatedAvgDelay;
                    minCity = aToCInfo.Value.Destination;
                }
            }
            result = minCity == null ? Result.NoConnectionFlightsFromAToB : Result.Success;
            return new Tuple<City, decimal>(minCity, minAvgArrDelay);
        }
    }
}
