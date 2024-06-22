using System.Collections.Generic;
using UnityEngine;

namespace GridSystem
{
    public interface IGameBoard
    {
        public IGridSlot this[int row, int column] { get; }
        public IGridSlot this[Vector2Int position] { get; }
        public int Width { get; }
        public int Height { get; }
        public List<IGridSlot> GridSlots1D { get; }

        IGridSlot GetGridSlot(Vector2Int position);
        Vector3 GridToWorldCenter(Vector2Int gridPosition);
        Vector2Int WorldToGrid(Vector3 worldPosition);
    }
}
