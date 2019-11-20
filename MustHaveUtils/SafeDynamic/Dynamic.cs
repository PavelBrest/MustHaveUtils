using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MustHaveUtils.SafeDynamic
{
    public class Dynamic
    {
        private object _value;
        private MethodInfo[] _currentMethods;
        private readonly HashSet<Type> _types;

        private Dynamic(HashSet<Type> types)
        {
            _types = types;
        }

        public object Value
        {
            get => _value;
            set
            {
                _value = value;
                CurrentType = value.GetType();
                _currentMethods = CurrentType.GetMethods();
            }
        }
        public Type CurrentType { get; private set; }
        
        public void Set<T>(T value)
        {
            if (!_types.TryGetValue(value.GetType(), out _))
                throw new InvalidOperationException($"Type {value.GetType().FullName} is missing.");

            Value = value;
        }

        public bool TrySet<T>(T value)
        {
            if (!_types.TryGetValue(value.GetType(), out _))
                return false;

            Value = value;
            return true;
        }
        
        public Optional<T> TryGet<T>()
        {
            if (_value == null || !_types.TryGetValue(typeof(T), out _) || CurrentType != typeof(T))
                return Optional.Empty<T>();
            
            return Optional.Of((T)_value);
        }
        
        public T Get<T>()
        {
            if (_value == null)
                throw new InvalidOperationException("Value not set yet.");
                
            if (!_types.TryGetValue(typeof(T), out _))
                throw new InvalidOperationException($"Type {typeof(T).FullName} is missing.");
            
            if (CurrentType != typeof(T))
                throw new InvalidOperationException($"Current type is {CurrentType.FullName}.");
            
            return (T)_value;
        }

        public static Dynamic Create<T, T1, T2, T3>() => Create(typeof(T), typeof(T1), typeof(T2), typeof(T3));
        public static Dynamic Create<T, T1, T2>() => Create(typeof(T), typeof(T1), typeof(T2));
        public static Dynamic Create<T, T1>() => Create(typeof(T), typeof(T1));
        public static Dynamic Create<T>() => Create(typeof(T));

        public static Dynamic Create(params Type[] types)
        {
            if (types == null) throw new ArgumentNullException(nameof(types));
            if (!types.Any()) throw new InvalidOperationException($"{nameof(types)} is empty.");
            
            var set = new HashSet<Type>(types.Length);
            
            foreach (var type in types)
            {
                if (!set.Add(type))
                    throw new InvalidOperationException($"Type {type.FullName} already added.");
            }
            
            return new Dynamic(set);
        }
    }
}