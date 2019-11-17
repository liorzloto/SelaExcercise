namespace SelaExercise
{
    public class Airline : NamedObject
    {
        public Airline(string name) : base(name)
        {
        }

        public override bool Equals(object obj)
        {
            return NameEquals<Airline>(obj);
        }

        public static bool operator ==(Airline obj1, Airline obj2)
        {
            return obj1.Equals(obj2);
        }

        public static bool operator !=(Airline obj1, Airline obj2)
        {
            return !obj1.Equals(obj2);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
