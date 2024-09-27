namespace Dunward.Capricorn
{
    public class Ref<T>
    {
        public T Value { get; set; }

        public Ref(T value)
        {
            Value = value;
        }

        public static implicit operator T(Ref<T> reference)
        {
            return reference.Value;
        }

        public static implicit operator Ref<T>(T value)
        {
            return new Ref<T>(value);
        }
    }
}