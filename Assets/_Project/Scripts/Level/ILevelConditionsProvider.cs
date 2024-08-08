using Observable;
using System;
using System.Collections.Generic;

namespace App
{
    public interface ILevelConditionsProvider
    {
        event Action OnMoveReachedZero;
        List<ILevelGoal> LevelGoals { get; }
        Observable<int> MoveCount { get; }
        bool TryGetGoal(int id, out ILevelGoal goal);
        bool IsGoal(int id);
        void DecreaseMove();
    }

}

