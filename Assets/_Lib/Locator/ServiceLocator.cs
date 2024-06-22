using System;
using System.Collections.Generic;
using UnityEngine;

namespace Service_Locator
{
    public class ServiceLocator : IServiceLocator
    {
        private readonly Dictionary<Type, object> _services = new();
        public void Register<T>(T service) where T : class
        {
            var type = typeof(T);

            if (!_services.TryAdd(type, service))
            {
                Debug.LogError($"Service of {type} is already registered");
            }
        }

        public T Get<T>() where T : class
        {
            var type = typeof(T);

            if (!_services.TryGetValue(type, out var service))
            {
                throw new KeyNotFoundException($"Service of {type} is not registered");
            }
            return service as T;
        }
        public bool TryGet<T>(out T service) where T : class
        {
            var type = typeof(T);
            if (_services.TryGetValue(type, out var obj))
            {
                service = obj as T;
                return true;
            }
            service = null;
            return false;
        }
        public bool IsRegistered<T>()
        {
            var type = typeof(T);

            return _services.ContainsKey(type);
        }
    }
}