using System;

namespace SelaExercise
{
    public class Program
    {
        static void Main(string[] args)
        {
            var flightsInfo = ExcelReader.ReadCSV(Tools.CSV_FILE_NAME);

            bool continueLoop;
            string actionChosenString;
            int actionChosenNumber;
            string message;
            (string, Func<string[], FlightsInfo, string>, int) actionInfo;
            var actionsInfo = Actions.ActionsInfo;
            do
            {
                Console.WriteLine();
                Console.WriteLine("Choose action (by its number):");
                foreach (var action in actionsInfo)
                    Console.WriteLine(string.Format("{0} - {1}", action.Key, action.Value.Item1));

                actionChosenString = Console.ReadLine();
                if (int.TryParse(actionChosenString, out actionChosenNumber) && 
                    Actions.ActionsInfo.ContainsKey(actionChosenNumber))
                {
                    actionInfo = Actions.ActionsInfo[actionChosenNumber];
                    message = actionInfo.Item2.Invoke(GetInputs(actionInfo.Item3), flightsInfo);
                    if (message == null)
                        continueLoop = false;
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine(message);
                        continueLoop = true;
                    }
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid input {0}. Try again.", actionChosenString);
                    continueLoop = true;
                }
            } while (continueLoop);
        }

        private static string[] GetInputs(int numberOfInputs)
        {
            var inputs = new string[numberOfInputs];
            for (int i = 0; i < numberOfInputs; i++)
            {
                Console.WriteLine("\nEnter City {0}", (char)(i + 'A'));
                inputs[i] = Console.ReadLine();
            }
            return inputs;
        }
    }
}
