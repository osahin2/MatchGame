using Item;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    [CreateAssetMenu]
    public class LevelData : ScriptableObject
    {
        public List<ItemData> FallItems = new();
        public List<LevelGoalData> LevelGoals = new();
        public List<LevelGridData> LevelGridData = new();
        public int MoveCount;
        public int GridWidth;
        public int GridHeigth;

        private readonly Dictionary<Vector2Int, LevelGridData> _gridDataDict = new();
        public void Construct()
        {
            foreach (var item in LevelGridData)
            {
                _gridDataDict.Add(item.GridPosition, item);
            }
        }
        public void DeConstruct()
        {
            _gridDataDict.Clear();
        }
        public LevelGridData GetGridData(Vector2Int pos)
        {
            if(_gridDataDict.TryGetValue(pos, out var data)) 
                return data;

            throw new KeyNotFoundException($"{pos} Grid Not Found In LevelDataGrid Dict: {GetType().Name}");
        }
    }
    [Serializable]
    public struct LevelGridData
    {
        public ItemData itemData;
        public Vector2Int GridPosition;
    }
    [Serializable]
    public struct LevelGoalData
    {
        public ItemData itemData;
        public int Goal;
    }
}

