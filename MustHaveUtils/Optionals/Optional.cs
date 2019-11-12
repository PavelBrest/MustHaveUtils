using System;
using System.Diagnostics.CodeAnalysis;

#nullable enable
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

        internal Optional() => 
            (_value, HasValue) = (default!, false);

        internal Optional(TValue value) =>
            (_value, HasValue) = (value ?? throw new ArgumentNullException(nameof(value)), true);

        public void IfPresent([NotNull] Action<TValue> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            if (HasValue)
                action.Invoke(_value);
        }

        public TValue OrElseGet([NotNull] Func<TValue> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            return HasValue ? 
                _value : 
                action.Invoke();
        }

        public TValue OrElse(TValue value)
            => HasValue ? _value : value;

        public Optional<TValue> Where([NotNull] Func<TValue, bool> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return !HasValue || predicate.Invoke(_value) ?
                this :
                Empty<TValue>();
        }

        public Optional<T> Map<T>([NotNull] Func<TValue, T> func)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));

            return HasValue ?
                OfNullable(func.Invoke(_value)) :
                Empty<T>();
        }

        public static explicit operator TValue(Optional<TValue> optional)
            => optional.GetValue;

        public static explicit operator Optional<TValue>(TValue value)
            => Optional.Of(value);

        public override bool Equals(object? obj)
        {
            return obj is Optional<TValue> && Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool Equals(Optional<TValue> other)
        {
            if (other == null)
                return false;

            return HasValue && other.HasValue ?
                Equals(_value, other.GetValue) :
                HasValue == other.HasValue;
        }
    }
}
