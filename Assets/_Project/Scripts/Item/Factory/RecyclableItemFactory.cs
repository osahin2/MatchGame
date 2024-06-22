using Pool;
using System.Collections.Generic;
using UnityEngine;
namespace Item
{
    public class RecyclableItemFactory : IItemFactory
    {
        private readonly List<GenericItemFactory> _itemFactories = new();

        private readonly Transform _parent;

        private readonly Dictionary<ItemType, GenericItemFactory> _factoriesDict = new();
        public RecyclableItemFactory(List<GenericItemFactory> factories)
        {
            _itemFactories = factories;
            _parent = new GameObject("ItemsParent").transform;

            ConstructAndAddFactoriesToDict();
        }
        private void ConstructAndAddFactoriesToDict()
        {
            foreach (var factory in _itemFactories)
            {
                factory.Construct(_parent);
                _factoriesDict.Add(factory.Type, factory);
            }
        }
        public IItem Get(ItemType itemType)
        {
            return GetItem(itemType);
        }
        public void Free(IItem item)
        {
            GetFactory(item.Type).Free(item);
        }
        private IItem GetItem(ItemType itemType)
        {
            return GetFactory(itemType).Get();
        }
        private GenericItemFactory GetFactory(ItemType itemType)
        {
            if(!_factoriesDict.TryGetValue(itemType, out var factory))
            {
                throw new KeyNotFoundException($"{itemType} Not Found In Factory Dictionary");
            }
            return factory;
        }
    }
}