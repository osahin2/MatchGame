using Pool;
using UnityEngine;
namespace Item
{
    public abstract class GenericItemFactoryBase : ScriptableObject
    {
        [SerializeField] private ItemBase _item;
        [SerializeField] private int _initialSpawnCount;

        private IPooler<IItem> _pooler;

        private Transform _spawnParent;
        public void Construct(Transform spawnParent)
        {
            _spawnParent = spawnParent;

            _pooler = new Pooler<IItem>.Builder(CreateFunc)
                .WithInitialCount(_initialSpawnCount)
                .WithOnInitialSpawn(OnInitiallySpawned)
                .Build();
        }
        public IItem Get()
        {
            return _pooler.GetPooled();
        }
        public void Free(IItem item)
        {
            _pooler.Free(item);
        }
        private IItem CreateFunc()
        {
            var item = Instantiate(_item, _spawnParent);
            return item;
        }
        private void OnInitiallySpawned(IItem item)
        {
            item.Hide();
        }

    }
}