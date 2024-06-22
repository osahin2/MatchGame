using UnityEngine;
namespace Item
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Match2/Item/Item Data")]
    public class ItemData : ScriptableObject
    {
        [SerializeField] private Sprite _icon;
        [SerializeField] private ItemType _itemType;
        [SerializeField] private bool _isPoppable;
        public Sprite Icon => _icon;
        public ItemType ItemType => _itemType;
        public int ID => GetInstanceID();
        public bool IsPoppable => _isPoppable;
    }
}