using Item;
using System.Collections.Generic;
using UnityEngine;

namespace Power
{
    public class PowerTypeProvider : IPowerTypeProvider
    {
        public PowerType GetPowerType(ItemType itemType) => itemType switch
        {
            ItemType.Rocket => GetRandomRocketType(),
            _ => throw new System.NotImplementedException()
        };
        private PowerType GetRandomRocketType()
        {
            var rocketTypes = new List<PowerType> { PowerType.VerticalRocket, PowerType.HorizontalRocket };
            return rocketTypes[Random.Range(0, rocketTypes.Count)];
        }

    }
}

