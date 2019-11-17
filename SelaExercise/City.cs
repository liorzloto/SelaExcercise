namespace SelaExercise
{
    public class City : NamedObject
    {
        public City(string name) : base(name)
        {
        }

        public override bool Equals(object obj)
        {
            return NameEquals<City>(obj);
        }

        public static bool operator ==(City obj1, City obj2)
        {
            return obj1.Equals(obj2);
        }

        public static bool operator !=(City obj1, City obj2)
        {
            return !obj1.Equals(obj2);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
