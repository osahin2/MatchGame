using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    [CreateAssetMenu(fileName = "ItemContainer", menuName = "Match2/Item/ItemContainer")]
    public class ItemsContainer : ScriptableObject
    {
        [SerializeField] private List<ItemData> _items = new();
        public List<ItemData> Items => _items;

        private readonly Dictionary<int, ItemData> _itemDatas = new();

        public void Construct()
        {
            SetDictionary();
        }
        private void SetDictionary()
        {
            foreach (var item in _items)
            {
                _itemDatas.Add(item.ID, item);
            }
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