using System.Linq;

namespace SelaExercise
{
    public partial class FlightsInfo
    {
        /// <summary>
        /// Calculates average arrival delay from city A to city B through city C
        /// </summary>
        /// <param name="aToCInfo">An object containing information about flights from from city A to city C</param>
        /// <param name="destB">Name of city B</param>
        /// <returns>Average arrival delay from city A to city B through city C</returns>
        private decimal GetAvgArrivalDelayThroughCityC(OriginToDestinationInfo aToCInfo, string destB)
        {
            var cToBFlights = origins[aToCInfo.Destination.Name].Destinations[destB].Flights.Where(f => f.ArrivalDelay.HasValue).OrderBy(f => f.DepartureDate).ToArray();

            // Creates partial ascending and descending sums of arrival delays from sorted (by arrival delay)
            // array of flights from city C to city B
            var partialSums = GetAscendingAndDescendingPartialSumsOfArrivalDelays(cToBFlights);
            var partialSumsAscending = partialSums.Item1;
            var partialSumsDescending = partialSums.Item2;

            // Sort flights from city A to city C by actual arrival date
            var aToCFlights = aToCInfo.Flights.OrderBy(f => f.ActualArrivalDate).ToArray();
            var partialSumsAscendingIndexes = new int[aToCFlights.Length];
            var partialSumsDescendingIndexes = new int[aToCFlights.Length];
            int i;
            int j = 0;
            double cToBDepMinusAToCArr;

            // Foreach flight from city A to city C, mark valid continuation flights from city C to City B according
            // to difference between departure time of the continuation flight and planned arrival time of the first flight
            for (i = 0; i < aToCFlights.Length; i++)
            {
                while (j < cToBFlights.Length &&
                    (aToCFlights[i].ActualArrivalDate.AddHours(1) > cToBFlights[j].DepartureDate))
                    j++;
                partialSumsAscendingIndexes[i] = j;
            }

            // Sort flights from city A to city C by planned arrival date
            aToCFlights = aToCInfo.Flights.OrderBy(f => f.ArrivalDate).ToArray();
            j = cToBFlights.Length - 1;

            // Foreach flight from city A to city C, mark valid continuation flights from city C to City B according
            // to difference between departure time of the continuation flight and planned arrival time of the first flight
            for (i = aToCFlights.Length - 1; i >= 0; i--)
            {
                if (j >= 0)
                {
                    cToBDepMinusAToCArr = (cToBFlights[j].DepartureDate - aToCFlights[i].ArrivalDate).TotalHours;
                    while (j >= 0 && cToBDepMinusAToCArr >= 24)
                        j--;
                }
                partialSumsDescendingIndexes[i] = j + 1;
            }

            // Calculate average arrival delay by the partial sums arrays and the indexes of valid flights according
            // to the two conditions
            return CalculateAvgArrivalDelay(partialSumsDescending, partialSumsAscending,
                partialSumsAscendingIndexes, partialSumsDescendingIndexes);
        }

        private (int[], int[]) GetAscendingAndDescendingPartialSumsOfArrivalDelays(Flight[] flights)
        {
            var flightsLength = flights.Length;
            var partialSumsAscending = new int[flightsLength + 1];
            var partialSumsDescending = new int[flightsLength + 1];

            for (int i = 1; i <= flightsLength; i++)
            {
                partialSumsAscending[i] = partialSumsAscending[i - 1] + flights[i - 1].ArrivalDelay.Value;
                partialSumsDescending[flightsLength - i] =
                    partialSumsDescending[flightsLength - i + 1] + flights[flightsLength - i].ArrivalDelay.Value;
            }
            return (partialSumsAscending, partialSumsDescending);
        }

        private decimal CalculateAvgArrivalDelay(int[] partialSumsDescending, int[] partialSumsAscending,
                int[] partialSumsAscendingIndexes, int[] partialSumsDescendingIndexes)
        {
            var firstDescendingSum = partialSumsDescending[0];
            var numberOfValidJourneys = 0;
            var sumOfDelays = 0;
            int descendingIndex;
            int ascendingIndex;
            for (int i = 0; i < partialSumsDescendingIndexes.Length; i++)
            {
                descendingIndex = partialSumsDescendingIndexes[i];
                ascendingIndex = partialSumsAscendingIndexes[i];
                if (descendingIndex < ascendingIndex)
                {
                    numberOfValidJourneys += ascendingIndex - descendingIndex;
                    sumOfDelays += firstDescendingSum -
                        partialSumsAscending[ascendingIndex] - partialSumsDescending[descendingIndex];
                }
            }
            return numberOfValidJourneys > 0 ? decimal.Divide(sumOfDelays, numberOfValidJourneys) : -1;
        }
    }
}
