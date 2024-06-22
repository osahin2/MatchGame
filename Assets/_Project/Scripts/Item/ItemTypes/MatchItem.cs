using System;
using UnityEngine;
namespace Item
{
    public class MatchItem : ItemBase
    {
        public override void Interact(Action onSuccess)
        {
            onSuccess?.Invoke();
            Debug.Log("Match Item Interact");
        }
    }
}