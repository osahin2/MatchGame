using Level;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace UI
{
    public class InGameUIView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _movesText;
        [SerializeField] private List<LevelGoalUI> _levelGoals = new();
        
        public List<LevelGoalUI> ActivatedGoals { get; private set; } = new();
        public void Init(LevelData levelData)
        {
            SetLevelGoals(levelData.LevelGoals);
            SetMoveText(levelData.MoveCount);
        }
        public void DeInit()
        {
            foreach (var item in ActivatedGoals)
            {
                item.gameObject.SetActive(false);
            }
            ActivatedGoals.Clear();
        }
        private void SetLevelGoals(List<LevelGoalData> goals)
        {
            for (int i = 0; i < goals.Count; i++)
            {
                var goal = goals[i];
                var goalUI = _levelGoals[i];
                goalUI.gameObject.SetActive(true);
                goalUI.SetGoalInfo(goal.itemData, goal.Goal);
                ActivatedGoals.Add(goalUI);
            }
        }
        public void SetMoveText(int moveCount)
        {
            _movesText.text = moveCount.ToString();
        }
        public void UpdateLevelGoal(int id, int goal)
        {
            var targetLevelGoal = ActivatedGoals.Find(x => x.ID == id);

            if (targetLevelGoal != null)
            {
                targetLevelGoal.SetGoalText(goal);
                targetLevelGoal.Hit();
            }
        }
    }
}

