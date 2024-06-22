using Item;
using UnityEngine;
namespace GridSystem
{
    public interface IGridSlot
    {
        Vector2Int GridPosition { get; }
        IItem Item { get; }
        bool HasItem { get; }

        void SetItem(IItem item);
        void Clear();
    }
}
