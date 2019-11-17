namespace SelaExercise
{
    public abstract class NamedObject
    {
        public readonly string Name;

        public NamedObject(string name)
        {
            Name = name;
        }

        protected bool NameEquals<T>(object obj2) where T : NamedObject
        {
            return obj2 != null && obj2.GetType() == typeof(T) && ((T)this).Name == ((NamedObject)obj2).Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
