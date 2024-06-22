using System;
using System.Collections.Generic;

namespace Pool
{
    public class Pooler<T> : IPooler<T> where T : class
    {
        private Queue<T> _pooledObjects = new();
        private HashSet<T> _activatedObjects = new();

        private Func<T> _createFunc;
        private Action<T> _onFreeAction;
        private Action<T> _onGetAction;
        private Action<T> _onDestroyAction;
        private Action<T> _onInitialSpawnAction;

        private Pooler(Func<T> createFunc, Action<T> onGet, Action<T> onFree, Action<T> onDestroy, Action<T> onInitialSpawn, int count)
        {
            _createFunc = createFunc;
            _onGetAction = onGet;
            _onDestroyAction = onDestroy;
            _onFreeAction = onFree;
            _onInitialSpawnAction = onInitialSpawn;

            if (count == 0) return;

            for (int i = 0; i < count; i++)
            {
                var obj = _createFunc();
                _pooledObjects.Enqueue(obj);
                _onInitialSpawnAction?.Invoke(obj);
            }

        }

        public T GetPooled()
        {
            T obj;
            if (_pooledObjects.Count == 0)
            {
                obj = _createFunc();
                _activatedObjects.Add(obj);
            }
            else
            {
                obj = _pooledObjects.Dequeue();
                _activatedObjects.Add(obj);
            }
            _onGetAction?.Invoke(obj);
            return obj;
        }
        public void Free(T obj)
        {
            if (!_activatedObjects.Contains(obj)) return;

            _pooledObjects.Enqueue(obj);
            _activatedObjects.Remove(obj);

            _onFreeAction?.Invoke(obj);
        }

        public void Clear()
        {
            foreach (var item in _pooledObjects)
            {
                _onDestroyAction?.Invoke(item);
            }
            foreach (var item in _activatedObjects)
            {
                _onDestroyAction?.Invoke(item);
            }

            _pooledObjects.Clear();
            _activatedObjects.Clear();
        }

        public class Builder
        {
            private Func<T> _createFunc;
            private Action<T> _onFreeAction;
            private Action<T> _onGetAction;
            private Action<T> _onDestroyAction;
            private Action<T> _onInitialSpawnAction;
            private int _count = 0;

            public Builder(Func<T> createFunc)
            {
                _createFunc = createFunc;
            }
            public Builder WithInitialCount(int count)
            {
                _count = count;
                return this;
            }
            public Builder WithOnFree(Action<T> onFreeAction)
            {
                _onFreeAction = onFreeAction;
                return this;
            }
            public Builder WithOnGet(Action<T> onGet)
            {
                _onGetAction = onGet;
                return this;
            }
            public Builder WithOnDestroy(Action<T> onDestroy)
            {
                _onDestroyAction = onDestroy;
                return this;
            }
            public Builder WithOnInitialSpawn(Action<T> onInitialSpawn)
            {
                _onInitialSpawnAction = onInitialSpawn;
                return this;
            }
            public Pooler<T> Build()
            {
                return new Pooler<T>(
                    _createFunc,
                    _onGetAction,
                    _onFreeAction,
                    _onDestroyAction,
                    _onInitialSpawnAction,
                    _count);
            }
        }
    }
}