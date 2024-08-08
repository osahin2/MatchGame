using UnityEngine;
namespace Item
{
    [CreateAssetMenu(fileName = "GenericItemFactory", menuName = "Match2/Item/Generic Item Factory")]
    public class GenericItemFactory : GenericItemFactoryBase
    {
        [SerializeField] private ItemType _itemType;
        public ItemType ItemType => _itemType;
    }
}