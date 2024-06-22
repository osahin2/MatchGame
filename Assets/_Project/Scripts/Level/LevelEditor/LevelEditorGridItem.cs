using Item;
using UnityEngine;

namespace Level
{
#if UNITY_EDITOR
    public class LevelEditorGridItem : MonoBehaviour
    {
        public SpriteRenderer SpriteRenderer;
        public int ID;
        public Vector2Int GridPos;
        public ItemType ItemType;
    }
#endif
}

