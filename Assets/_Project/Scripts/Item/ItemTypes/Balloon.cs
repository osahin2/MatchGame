using Audio;
using System;
using UnityEngine;
namespace Item
{
    public class Balloon : SpecialItemBase
    {
        public override void CheckSpecial(Action onSuccess)
        {
        }

        public override void Interact()
        {
            ShakeAnim();
        }

        public override void Pop()
        {
            Audio.Play(ClipType.Balloon);
        }
    }
}