using System;

namespace SelaExercise
{
    public class Flight
    {
        public readonly City Origin;
        public readonly City Destination;
        public readonly Airline Airline;
        public readonly DateTime DepartureDate;
        public readonly DateTime ArrivalDate;
        public readonly int? DepartureDelay;
        public readonly int? ArrivalDelay;
        public readonly int Distance;
        public DateTime ActualArrivalDate { 
            get { return ArrivalDate.AddMinutes(ArrivalDelay.HasValue ? ArrivalDelay.Value : 0); } }

        public Flight(City origin, City destination, Airline airline, DateTime depDate, DateTime arrDate,
            int? depDelay, int? arrDelay, int distance)
        {
            Origin = origin;
            Destination = destination;
            Airline = airline;
            DepartureDate = depDate;
            ArrivalDate = arrDate;
            DepartureDelay = depDelay;
            ArrivalDelay = arrDelay;
            Distance = distance;
        }
    }
}
