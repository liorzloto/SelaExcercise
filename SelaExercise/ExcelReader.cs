using System;
using System.Collections.Generic;
using Microsoft.VisualBasic.FileIO;

namespace SelaExercise
{
    public class ExcelReader
    {
        private const string COMMENT_TOKEN = "#";
        private const string DELIMITER = ",";
        private const string NO_DATA = "NA";

        /// <summary>
        /// Maps names of fields to their indexes in the file
        /// </summary>
        private static readonly IDictionary<string, int> FIELDS = new Dictionary<string, int>()
        {
            { "Origin", 16 }, { "Destination", 25 }, { "Airline", 9 }, { "DepDate", 6 }, { "CRSDepTime", 30 },
            { "DepDelay", 32 }, { "CRSElapsedTime", 51 }, { "ArrDelay", 43 }, { "Distance", 55 }
        };

        public static FlightsInfo ReadCSV(string path)
        {
            
            using TextFieldParser csvParser = new TextFieldParser(path);
            csvParser.CommentTokens = new string[] { COMMENT_TOKEN };
            csvParser.SetDelimiters(new string[] { DELIMITER });
            csvParser.HasFieldsEnclosedInQuotes = true;

            // Skip the row with the column names
            csvParser.ReadLine();

            var flightsInfo = new FlightsInfo();
            string[] fields;
            var counter = 0;
            while (!csvParser.EndOfData)
            {
                // Read current line fields, pointer moves to the next line
                fields = csvParser.ReadFields();
                CreateFlight(fields, flightsInfo);
                if (++counter % 10000 == 0)
                    Console.WriteLine("Read {0} lines", counter);
            }

            if (counter % 10000 != 0)
                Console.WriteLine("Read {0} lines", counter);
            return flightsInfo;
        }

        /// <summary>
        /// Based on readen data, creates flight object and adds it to a FlightsInfo object
        /// </summary>
        /// <param name="fields">Readen data fields</param>
        /// <param name="flightsInfo">FlightsInfo object to add the new flight to</param>
        private static void CreateFlight(string[] fields, FlightsInfo flightsInfo)
        {
            var origin = GetField(fields, "Origin");
            var dest = GetField(fields, "Destination");
            var airline = GetField(fields, "Airline");
            var depDate = ConvertToDate(GetField(fields, "DepDate"));
            var depTime = ConvertToMinutes(GetField(fields, "CRSDepTime"));
            var depDelay = ConvertToInt(GetField(fields, "DepDelay"));
            var crsElapsedTime = ConvertToMinutes(GetField(fields, "CRSElapsedTime"));
            var arrDelay = ConvertToInt(GetField(fields, "ArrDelay"));
            var distance = ConvertToInt(GetField(fields, "Distance"));

            if (depDate == DateTime.MinValue || depTime == -1 || crsElapsedTime == -1 || !distance.HasValue)
                return;

            flightsInfo.AddFlight(origin, dest, airline, depDate.AddMinutes(depTime), 
                depDate.AddMinutes(depTime).AddMinutes(crsElapsedTime), depDelay, arrDelay, distance.Value);
        }

        private static string GetField(string[] fields, string field)
        {
            return fields[FIELDS[field]];
        }

        private static int? ConvertToInt(string data)
        {
            return data == NO_DATA ? null : new int?(int.Parse(data));
        }

        private static DateTime ConvertToDate(string data)
        {
            return data == NO_DATA ? DateTime.MinValue : DateTime.Parse(data);
        }

        private static int ConvertToMinutes(string data)
        {
            if (data == NO_DATA || data.Length > 4 || data.Length == 0)
                return -1;
            var len = data.Length;
            var minutes = ConvertCharToDigit(data[len - 1]);
            if (len == 1)
                return minutes;
            else
                minutes += 10 * ConvertCharToDigit(data[len - 2]);
            if (len == 2)
                return minutes;
            else
                minutes += 60 * ConvertCharToDigit(data[len - 3]);
            if (len == 3)
                return minutes;
            else
                return minutes += 600 * ConvertCharToDigit(data[0]);
        }

        private static int ConvertCharToDigit(char c)
        {
            return c - '0';
        }
    }
}
