using UnityEngine;

namespace Utilities
{
    public static class GridUtility
    {
        public static bool IsPositionOnGrid(Vector2Int gridPosition, int width, int heigth)
        {
            return gridPosition.x >= 0 &&
                   gridPosition.x < width &&
                   gridPosition.y >= 0 &&
                   gridPosition.y < heigth;
        }
    }
}