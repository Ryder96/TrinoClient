namespace TrinoClient.Model
{
    public class Optional : Optional<object>
    {
        #region Private Fields

        private static readonly Optional EMPTY = null;

        #endregion

        new public static Optional Empty()
        {
            return Optional.EMPTY;
        }
    }

    /// <summary>
    /// From java.util.Optional
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Optional<T>
    {
        #region Private Fields

        private static readonly Optional<T> EMPTY = OfNullable(default(T));

        #endregion

        #region Public Properties

        public T Value { get; }

        #endregion

        #region Constructors

        public Optional()
        {
            Value = default(T);
        }

        public Optional(T value)
        {
            Value = value;
        }

        #endregion

        #region Static Public Methods

        public static Optional<T> Empty()
        {
            return EMPTY;
        }

        public static Optional<T> Of(T value)
        {
            return new Optional<T>(value);
        }

        public static Optional<T> OfNullable(T value)
        {
            return (value == null) ? Empty() : Of(value);
        }

        #endregion

        #region Public Methods

        public bool IsPresent()
        {
            return Value != null;
        }

        public T Get()
        {
            return Value;
        }

        public override bool Equals(object obj)
        {
            if (obj is Optional<T>)
            {
                return Equals((Optional<T>)obj);
            }
            else
            {
                return false;
            }
        }

        public bool Equals(Optional<T> other)
        {
            if (IsPresent() && other.IsPresent())
            {
                return Get().Equals(other.Get());
            }
            else
            {
                return IsPresent() == other.IsPresent();
            }
        }

        public override int GetHashCode()
        {
            if (Value != null)
            {
                return Hashing.Hash(Value);
            }
            else
            {
                return base.GetHashCode();
            }
        }

        #endregion
    }
}
