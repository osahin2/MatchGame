using Power;
using UnityEngine;
namespace Item
{
    [CreateAssetMenu(fileName = "Power Item Factory", menuName = "Match2/Item/Power Item Factory")]
    public class GenericPowerItemFactory : GenericItemFactoryBase
    {
        [SerializeField] private PowerType _powerType;
        public PowerType PowerType => _powerType;
    }
}