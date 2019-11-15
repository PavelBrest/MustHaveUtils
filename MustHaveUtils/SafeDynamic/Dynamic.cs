using System;
using System.Collections.Generic;
using System.Linq;

namespace MustHaveUtils.SafeDynamic
{
    public class Dynamic
    {
        private dynamic _value;
        private readonly HashSet<Type> _types;

        private Dynamic(HashSet<Type> types)
        {
            _types = types;
        }

        public Type CurrentType => _value?.GetType();

        public void Set<T>(T value)
        {
            if (!_types.TryGetValue(value.GetType(), out _))
                throw new InvalidOperationException($"Type {value.GetType().FullName} is missing.");

            _value = value;
        }

        public bool TrySet<T>(T value)
        {
            if (!_types.TryGetValue(value.GetType(), out _))
                return false;

            _value = value;
            return true;
        }
        
        public bool TryGet<T>(out T val)
        {
            val = default;
            
            if (CurrentType == null || !_types.TryGetValue(typeof(T), out _) || !_value.GetType().Equals(typeof(T)))
                return false;
            
            val = _value;
            return true;
        }
        
        public T Get<T>()
        {
            if (CurrentType == null)
                throw new InvalidOperationException("Value not set yet.");
                
            if (!_types.TryGetValue(typeof(T), out _))
                throw new InvalidOperationException($"Type {typeof(T).FullName} is missing.");
            
            if (!_value.GetType().Equals(typeof(T)))
                throw new InvalidOperationException($"Current type is {CurrentType.FullName}.");
            
            return _value;
        }

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