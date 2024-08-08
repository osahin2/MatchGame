using Observable;
using System;

namespace App
{
    public class LevelGoal : ILevelGoal
    {
        public event EventHandler<LevelGoalEventArgs> OnGoalChanged;
        public event Action OnReachedZero;
        public Observable<int> Goal { get; }
        public int ID { get; }
        public bool IsAchieved { get; private set; }

        public LevelGoal(int goal, int id)
        {
            Goal = new Observable<int>(goal, OnGoalSet);
            ID = id;
        }
        public void DecreaseGoal()
        {
            if (IsAchieved)
                return;

            if(Goal.Value - 1 <= 0)
            {
                IsAchieved = true;
                Goal.Value = 0;
                OnReachedZero?.Invoke();
                return;
            }
            Goal.Value -= 1;
        }
        private void OnGoalSet(int goal)
        {
            OnGoalChanged?.Invoke(this, new LevelGoalEventArgs(goal, ID));
        }

    }

}

