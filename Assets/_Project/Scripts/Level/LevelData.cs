using Item;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    [CreateAssetMenu]
    public class LevelData : ScriptableObject
    {
        public List<int> FallItems = new();
        public List<LevelGoal> LevelGoals = new();
        public List<LevelGridData> LevelGridData = new();
        public int GridWidth;
        public int GridHeigth;
    }
    [Serializable]
    public struct LevelGridData
    {
        public int ID;
        public Vector2Int GridPosition;
        public ItemType ItemType;
    }
}

