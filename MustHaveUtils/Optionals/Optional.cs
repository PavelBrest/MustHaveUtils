using System;

namespace MustHaveUtils
{
    public abstract class Optional
    {
        public static Optional<T> Empty<T>()
            => new Optional<T>();

        public static Optional<T> Of<T>(T value)
            => new Optional<T>(value);

        public static Optional<T> OfNullable<T>(T value)
            => value == null ? Empty<T>() : Of(value);
    }

    public sealed class Optional<TValue> : Optional
    {
        private readonly TValue _value;

        public bool HasValue { get; }
        public TValue GetValue => HasValue ?
            _value :
            throw new InvalidOperationException();

        internal Optional()
        {
            _value = default;
            HasValue = false;
        }

        internal Optional(TValue value)
        {
            _value = value;
            HasValue = true;
        }

        public void IfPresent(Action<TValue> action)
        {
            if (HasValue)
                action?.Invoke(_value);
        }

        public TValue OrElseGet(Func<TValue> action)
            => HasValue ? _value : action.Invoke();

        public TValue OrElse(TValue value)
            => HasValue ? _value : value;

        public Optional<TValue> Where(Func<TValue, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            if (!HasValue)
                return this;

            return predicate.Invoke(_value) ? this : Empty<TValue>();
        }

        public Optional<UValue> Map<UValue>(Func<TValue, UValue> func)
            => HasValue ?
                OfNullable(func.Invoke(_value)) :
                Empty<UValue>();

        public static explicit operator TValue(Optional<TValue> optional)
            => optional.GetValue;

        public static explicit operator Optional<TValue>(TValue value)
            => new Optional<TValue>(value);

        public override bool Equals(object obj)
        {
            if (obj is Optional<TValue>)
                return Equals(obj);
            else
                return false;
        }

        public bool Equals(Optional<TValue> other)
        {
            if (HasValue && other.HasValue)
                return Equals(_value, other.GetValue);
            else
                return HasValue == other.HasValue;
        }
    }
}
