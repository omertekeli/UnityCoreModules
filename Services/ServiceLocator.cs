using System;
using System.Collections.Generic;

namespace UnityCoreModules.Services
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

        public static void Register<T>(T service, bool replaceIfExists = false)
        {
            if (_services.ContainsKey(typeof(T)))
            {
                if (!replaceIfExists)
                    return;
            }
            _services[typeof(T)] = service;
        }
            
        public static void Unregister<T>() => _services.Remove(typeof(T));
        public static void ClearAll() => _services.Clear();
        public static T Get<T>()
        {
            if (_services.TryGetValue(typeof(T), out var service))
                return (T)service;
            throw new Exception($"Service of type {typeof(T)} not found.");
        }
    }
}