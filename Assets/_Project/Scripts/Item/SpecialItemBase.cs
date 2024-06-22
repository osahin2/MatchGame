using System;

namespace Item
{
    public abstract class SpecialItemBase : ItemBase
    {
        public abstract void CheckSpecial(Action onSuccess);
    }
}