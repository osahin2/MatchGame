using Level;
using Observable;
using System;
using System.Collections.Generic;

namespace App
{
    public class LevelConditionsProvider : ILevelConditionsProvider
    {
        public event Action OnMoveReachedZero;
        public List<ILevelGoal> LevelGoals { get; private set; } = new();
        public Observable<int> MoveCount { get; private set; }

        public void Init(LevelData levelData)
        {
            foreach (var data in levelData.LevelGoals)
            {
                LevelGoals.Add(new LevelGoal(data.Goal, data.itemData.ID));
            }
            MoveCount = new Observable<int>(levelData.MoveCount);
        }
        public void DeInit()
        {
            LevelGoals.Clear();
        }
        public void DecreaseMove()
        {
            MoveCount.Set(MoveCount - 1);
            if(MoveCount.Value == 0)
            {
                OnMoveReachedZero?.Invoke();
            }
        }
        public bool IsGoal(int id)
        {
            var goal = LevelGoals.Find(x => x.ID == id);
            return goal != null;
        }
        public bool TryGetGoal(int id, out ILevelGoal levelGoal)
        {
            var goal = LevelGoals.Find(x => x.ID == id);
            if(goal != null)
            {
                levelGoal = goal;
                return true;
            }

            levelGoal = null;
            return false;
        }
    }

}

