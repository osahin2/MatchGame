using Level;
using System.Collections.Generic;
using UnityEngine;

namespace App
{
    public class LevelController : MonoBehaviour, ILevel
    {
        [SerializeField] private List<LevelData> _levelDatas = new();

        private int _currentLevelIndex;

        public void Construct()
        {
            foreach (var levelData in _levelDatas)
            {
                levelData.Construct();
            }
        }
        public LevelData GetLevelData() 
        {
            return _levelDatas[_currentLevelIndex];
        }
        public void IncreaseLevel()
        {
            _currentLevelIndex++;
            if(_currentLevelIndex >= _levelDatas.Count)
            {
                _currentLevelIndex = 0;
            }
        }
    }
    
}

