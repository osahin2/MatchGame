using Item;

namespace Power
{
    public interface IPowerTypeProvider
    {
        PowerType GetPowerType(ItemType itemType);
    }
}

