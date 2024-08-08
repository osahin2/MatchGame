using Audio;
using System;
using UnityEngine;
namespace Item
{
    public class Duck : SpecialItemBase
    {
        public override void CheckSpecial(Action onSuccess)
        {
            var gridPos = GameBoard.WorldToGrid(transform.position);
            if (gridPos.y == 0)
            {
                Audio.Play(ClipType.Duck);
                onSuccess?.Invoke();
            }
        }

        public override void Interact()
        {
            ShakeAnim();
        }

        public override void Pop()
        {

        }
    }
}