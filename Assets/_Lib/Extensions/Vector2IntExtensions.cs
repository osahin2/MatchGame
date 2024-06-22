using UnityEngine;

namespace Extensions
{
    public static class Vector2IntExtensions
    {
        public static Vector2Int Left(this Vector2Int value)
        {
            return value + Vector2Int.left;
        }
        public static Vector2Int Right(this Vector2Int value)
        {
            return value + Vector2Int.right;
        }
        public static Vector2Int Up(this Vector2Int value)
        {
            return value + Vector2Int.up;
        }
        public static Vector2Int Down(this Vector2Int value)
        {
            return value + Vector2Int.down;
        }
    }
}