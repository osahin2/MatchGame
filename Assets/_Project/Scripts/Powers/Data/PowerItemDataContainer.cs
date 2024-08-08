using System.Collections.Generic;
using UnityEngine;
namespace Power
{
    [CreateAssetMenu(fileName = "Power Item Data Container", menuName ="Power/Power Item Container")]
    public class PowerItemDataContainer : ScriptableObject
    {
        [SerializeField] private List<PowerItemData> _itemDatas = new();

        private readonly Dictionary<PowerType, PowerItemData> _itemDatasDict = new();

        public void Construct()
        {
            foreach (var item in _itemDatas)
            {
                _itemDatasDict.Add(item.PowerType, item);
            }
        }
        public PowerItemData GetPowerItemData(PowerType powerType)
        {
            if(_itemDatasDict.TryGetValue(powerType, out PowerItemData powerItemData))
            {
                return powerItemData;
            }
            throw new KeyNotFoundException($"{powerType} Not Found In PowerItemData Dictionary");
        }
    }

}

