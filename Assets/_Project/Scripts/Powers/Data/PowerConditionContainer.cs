using Item;
using Service_Locator;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Power
{
    [CreateAssetMenu(fileName = "Power Conditions", menuName = "Power/Power Conditions")]
    public class PowerConditionContainer : ScriptableObject
    {
        [SerializeField] private List<PowerConditionData> _powerConditions = new();
        [SerializeField] private PowerItemDataContainer _powerItemDataContainer;

        private readonly Dictionary<int, PowerConditionData> _powerConditionsDict = new();
        private IPowerTypeProvider PowerTypeProvider => _powerTypeProvider
            ??= ServiceProvider.Instance.Get<IPowerTypeProvider>();

        private IPowerTypeProvider _powerTypeProvider;
        public void Construct()
        {
            foreach (var power in _powerConditions)
            {
                _powerConditionsDict.Add(power.condition, power);
            }
            _powerItemDataContainer.Construct();
        }
        public bool TryGetPower(int condition, out PowerItemData power)
        {
            var findPower = _powerConditions.FindLast(x => condition >= x.condition);
            if (findPower != null)
            {
                var powerType = PowerTypeProvider.GetPowerType(findPower.itemType);
                power = _powerItemDataContainer.GetPowerItemData(powerType);
                return true;
            }
            power = null;
            return false;
        }

        [Serializable]
        private class PowerConditionData
        {
            public int condition;
            public ItemType itemType;
        }
    }

}

