using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    [CreateAssetMenu(fileName = "ItemContainer", menuName = "Match2/Item/ItemContainer")]
    public class ItemsContainer : ScriptableObject
    {
        [SerializeField] private List<ItemData> _items = new();
        public List<ItemData> Items => _items;

        private readonly Dictionary<ItemType, List<ItemData>> _itemDataDict = new();
        private readonly Dictionary<int, ItemData> _itemDatas = new();

        public void Construct()
        {
            SetDictionary();
        }
        private void SetDictionary()
        {
            foreach (ItemData item in _items)
            {
                if(_itemDataDict.TryGetValue(item.ItemType, out List<ItemData> items))
                {
                    items.Add(item);
                    continue;
                }

                _itemDataDict.Add(item.ItemType, new List<ItemData> { item });
            }

            foreach (var item in _items)
            {
                _itemDatas.Add(item.ID, item);
            }
        }

        public ItemData GetItemData(ItemType type)
        {
            if(_itemDataDict.TryGetValue(type, out List<ItemData> items))
            {
                return items[Random.Range(0, items.Count)];
            }
            return null;
        }
        public ItemData GetItemDataById(int id)
        {
            if (_itemDatas.TryGetValue(id, out ItemData item))
            {
                return item;
            }
            return null;
        }
    }
}