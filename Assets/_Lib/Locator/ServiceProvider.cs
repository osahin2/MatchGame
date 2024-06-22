using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Service_Locator
{
    [DefaultExecutionOrder(-1)]
    public class ServiceProvider : MonoBehaviour
    {
        public static ServiceProvider Instance => _instance;
        private static ServiceProvider _instance;

        private IServiceLocator _locator;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            _locator = new ServiceLocator();
        }

        public ServiceProvider Register<T>(T service) where T : class
        {
            _locator.Register(service);
            return this;
        }

        public ServiceProvider Get<T>(out T service) where T : class
        {
            if (_locator.TryGet(out service))
            {
                return this;
            }
            throw new ArgumentException($"ServiceProvider.Get: Service of type {typeof(T).FullName} not registered");
        }
        public T Get<T>() where T : class
        {
            return _locator.Get<T>();
        }
        public bool IsRegistered<T>()
        {
            return _locator.IsRegistered<T>();
        }
#if UNITY_EDITOR

        [MenuItem("GameObject/ServiceLocator/ServiceProvider")]
        private static void AddProviderToScene()
        {
            var go = new GameObject("ServiceProvider", typeof(ServiceProvider));
            EditorUtility.SetDirty(go);
        }
#endif
    }
}