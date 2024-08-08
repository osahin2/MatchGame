using Power;

namespace Item
{
    public interface IItemFactory
    {
        IItem GetItem(ItemType itemType);
        IItem GetPowerItem(PowerType powerType);
        void FreeItem(IItem item);
    }
}