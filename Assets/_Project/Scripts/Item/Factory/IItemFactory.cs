namespace Item
{
    public interface IItemFactory
    {
        IItem Get(ItemType itemType);
        void Free(IItem item);
    }
}