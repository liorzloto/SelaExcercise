using System;
using System.Collections.Generic;
using System.Text;

namespace SelaExercise
{
    public static class Actions
    {
        /// <summary>
        /// Contains the numbers that have to be inserted to choose actions, and for each number the:
        /// Description of the action
        /// Function to be invoked to execute the action chosen
        /// The number of arguments the user has to insert to execute the action
        /// </summary>
        public static readonly SortedDictionary<int, (string, Func<string[], FlightsInfo, string>, int)> ActionsInfo = 
            new SortedDictionary<int, (string, Func<string[], FlightsInfo, string>, int)>()
            { 
                { 1, ("Find average departure and arrival delays from city A to city B", Action1, 2) }, 
                { 2, ("Find the airline with the most flights from city A", Action2, 1) }, 
                { 3, ("Find the 5 farthest destinations you could fly to from city A", Action3, 1) },
                { 4, ("From all the one-stop journeys from city A to city B through city C, find which is the journey with the minimal average arrival delay", Action4, 2) }, 
                { 5, ("Close application", Exit, 0) }
            };

        private static string Action1(string[] inputs, FlightsInfo flightsInfo)
        {
            Result result;
            var delays = flightsInfo.GetAvgDepartureAndArrivalDelays(inputs[0], inputs[1], out result);
            return result.IsSuccess ? string.Format("Based on {0} flights between {1} and {2}:\nThe average departure" +
                " delay is {3} minutes\nThe average arrival delay is {4} minutes", delays.Item3, inputs[0], inputs[1],
                delays.Item1.ToString("#.##"), delays.Item2.ToString("#.##")) : result.Message;
        }

        private static string Action2(string[] inputs, FlightsInfo flightsInfo)
        {
            Result result;
            var airline = flightsInfo.GetAirlineWithMostFlights(inputs[0], out result);
            return result.IsSuccess ? string.Format("The airline with the most flights from {0} is {1}, with {2} flights",
                inputs[0], airline.Item1, airline.Item2) : result.Message;
        }

        private static string Action3(string[] inputs, FlightsInfo flightsInfo)
        {
            Result result;
            var destinations = flightsInfo.GetFurthestDestinations(inputs[0], out result);
            if (!result.IsSuccess)
                return result.Message;
            var builder = new StringBuilder(string.Format("The farthest destinations from {0} are (in descending order): ",
                inputs[0]));
            for (int i = 0; i < destinations.Count; i++)
            {
                if (i != 0)
                    builder.Append(", ");
                builder.Append(string.Format("{0} ({1} miles)", destinations[i].Item1, destinations[i].Item2));
            }
            return builder.ToString();
        }

        private static string Action4(string[] inputs, FlightsInfo flightsInfo)
        {
            Result result;
            var returnValue = flightsInfo.GetJourneyWithMinimalAvgArrivalDelay(inputs[0], inputs[1], out result);
            return result.IsSuccess ? string.Format("The one-stop journey with the minimal arrival delay from {0} to {1}" +
                " is through {2}. It has average arrival delay of {3} minutes.", inputs[0], inputs[1], 
                returnValue.Item1, returnValue.Item2.ToString("#.##")) : result.Message;
        }

        private static string Exit(string[] inputs, FlightsInfo flightsInfo)
        {
            return null;
        }
    }
}
