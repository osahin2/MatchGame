using Pool;
using Power;
using System.Collections.Generic;
using UnityEngine;
namespace Item
{
    public class RecyclableItemFactory : IItemFactory
    {
        private readonly List<GenericItemFactory> _itemFactories = new();
        private readonly List<GenericPowerItemFactory> _powerItemFactories = new();

        private readonly Dictionary<ItemType, GenericItemFactory> _factoriesDict = new();
        private readonly Dictionary<PowerType, GenericPowerItemFactory> _powerFactoriesDict = new();

        private readonly Transform _parent;

        public RecyclableItemFactory(List<GenericItemFactory> factories, List<GenericPowerItemFactory> powerFactories)
        {
            _itemFactories = factories;
            _powerItemFactories = powerFactories;
            _parent = new GameObject("ItemsParent").transform;

            ConstructAndAddFactoriesToDict();
        }
        private void ConstructAndAddFactoriesToDict()
        {
            foreach (var factory in _itemFactories)
            {
                factory.Construct(_parent);
                _factoriesDict.Add(factory.ItemType, factory);
            }
            foreach (var factory in _powerItemFactories)
            {
                factory.Construct(_parent);
                _powerFactoriesDict.Add(factory.PowerType, factory);
            }
        }
        public IItem GetItem(ItemType itemType)
        {
            return GetItemFactory(itemType).Get();
        }
        public IItem GetPowerItem(PowerType powerType)
        {
            return GetPowerFactory(powerType).Get();
        }
        public void FreeItem(IItem item)
        {
            if (item.IsPower)
            {
                GetPowerFactory(item.PowerType).Free(item);
            }
            else
            {
                GetItemFactory(item.Type).Free(item);
            }
        }
        private GenericPowerItemFactory GetPowerFactory(PowerType powerType)
        {
            if (!_powerFactoriesDict.TryGetValue(powerType, out var factory))
            {
                throw new KeyNotFoundException($"{powerType} Not Found In Factory Dictionary");
            }
            return factory;
        }
        private GenericItemFactory GetItemFactory(ItemType itemType)
        {
            if(!_factoriesDict.TryGetValue(itemType, out var factory))
            {
                throw new KeyNotFoundException($"{itemType} Not Found In Factory Dictionary");
            }
            return factory;
        }
    }
}