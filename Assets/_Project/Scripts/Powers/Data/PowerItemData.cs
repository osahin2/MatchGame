using Item;
using UnityEngine;
namespace Power
{
    [CreateAssetMenu(fileName = "Power Item Data", menuName = "Power/Power Item Data")]
    public class PowerItemData : ScriptableObject
    {
        [SerializeField] private PowerType _powerType;
        [SerializeField] private ItemData _itemData;

        public PowerType PowerType => _powerType;
        public ItemData ItemData => _itemData;
    }

}

