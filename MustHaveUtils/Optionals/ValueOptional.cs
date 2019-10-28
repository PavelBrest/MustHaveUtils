using System;

namespace MustHaveUtils
{
    public struct ValueOptional<TValue>
        where TValue : struct
    {
        private static readonly ValueOptional<TValue> _empty = new ValueOptional<TValue>();

        private readonly TValue _value;

        public delegate void ValueIfPresentDelegate<T>(in T value)
            where T : struct;

        public delegate U ValueGetDelegate<U>()
            where U : struct;

        public delegate bool ValuePredicateDelegate<T>(in T value)
            where T : struct;

        public ValueOptional(in TValue value)
        {
            _value = value;
            HasValue = true;
        }

        public TValue Value { get => _value; }
        public bool HasValue { get; }

        public void IfPresent(ValueIfPresentDelegate<TValue> @delegate)
        {
            if (@delegate == null)
                throw new ArgumentNullException(nameof(@delegate));

            if (HasValue)
                @delegate.Invoke(in _value);
        }

        public TValue OrElseGet(ValueGetDelegate<TValue> @delegate)
        {
            if (@delegate == null)
                throw new ArgumentNullException(nameof(@delegate));

            return HasValue ? _value : @delegate.Invoke();
        }

        public ValueOptional<TValue> Where(ValuePredicateDelegate<TValue> @delegate)
        {
            if (@delegate == null)
                throw new ArgumentNullException(nameof(@delegate));

            if (!HasValue)
                return this;

            return @delegate.Invoke(in _value) ? this : Empty();
        }

        public TValue OrElse(in TValue value)
            => HasValue ? _value : value;

        public static ValueOptional<TValue> Empty()
            => _empty;

        public static ValueOptional<TValue> Of(in TValue value)
            => new ValueOptional<TValue>(value);
    }
}
