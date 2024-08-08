using System;

namespace App
{
    public class LevelGoalEventArgs : EventArgs
    {
        public int Goal { get; }
        public int ID { get; }
        public LevelGoalEventArgs(int goal, int id)
        {
            Goal = goal;
            ID = id;
        }
    }

}

