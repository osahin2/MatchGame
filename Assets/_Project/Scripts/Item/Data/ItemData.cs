using Power;
using System.Collections.Generic;
using UnityEngine;
namespace Item
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Match2/Item/Item Data")]
    public class ItemData : ScriptableObject
    {
        [SerializeField] private Sprite _icon;
        [SerializeField] private Color _itemColor;
        [SerializeField] private ItemType _itemType;
        [SerializeField] private bool _isPoppable;
        [SerializeField] private bool _isPower;
        public Sprite Icon => _icon;
        public ItemType ItemType => _itemType;
        public Color ItemColor => _itemColor;
        public int ID => GetInstanceID();
        public bool IsPoppable => _isPoppable;
        public bool IsPower => _isPower;
    }
}