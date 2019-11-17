namespace SelaExercise
{
    public class Result
    {
        private Result(string message) 
        { 
            Message = message; 
        }

        public readonly string Message;

        public bool IsSuccess { get { return Message == null; } }

        public static Result Success { get { return new Result(null); } }
        public static Result CityANotExist { get { return new Result("City A does not exist"); } }
        public static Result CityBNotExist { get { return new Result("City B does not exist"); } }
        public static Result NoFlightsFromCityA { get { return new Result("There are no flights from city A"); } }
        public static Result NoFlightsFromAToB { get { return new Result("There are no flights from city A to city B"); } }
        public static Result NoConnectionFlightsFromAToB { get { return new Result("There are no connection flights from city A to city B"); } }
    }
}
