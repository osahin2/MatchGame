using Observable;
using System;

namespace App
{
    public interface ILevelGoal
    {
        event EventHandler<LevelGoalEventArgs> OnGoalChanged;
        public Observable<int> Goal { get; }
        public int ID { get; }
        public bool IsAchieved { get; }
        void DecreaseGoal();
    }

}

