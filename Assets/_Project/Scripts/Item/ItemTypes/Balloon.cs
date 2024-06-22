using System;
using UnityEngine;
namespace Item
{
    public class Balloon : SpecialItemBase
    {
        public override void CheckSpecial(Action onSuccess)
        {
        }

        public override void Interact(Action onSuccess)
        {
            Debug.Log("Balloon Interact");
        }
    }
}