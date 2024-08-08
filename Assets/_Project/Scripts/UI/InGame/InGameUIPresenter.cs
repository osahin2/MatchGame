using App;
using Level;
using Service_Locator;
using System.Collections.Generic;

namespace UI
{
    public interface IUIGoals
    {
        public List<LevelGoalUI> LevelGoals { get; }
    }
    public class InGameUIPresenter : IUIGoals
    {
        private readonly InGameUIView _view;

        private ILevelConditionsProvider _conditionsProvider;

        public List<LevelGoalUI> LevelGoals => _view.ActivatedGoals;

        public InGameUIPresenter(InGameUIView view)
        {
            _view = view;
        }
        public void Bind()
        {
            ServiceProvider.Instance.Get(out  _conditionsProvider);
        }
        public void Init(LevelData levelData)
        {
            _view.Init(levelData);
            AddEvents();
        }
        public void DeInit()
        {
            RemoveEvents();
            _view.DeInit();
        }
        private void OnGoalChangedHandler(object sender, LevelGoalEventArgs args)
        {
            _view.UpdateLevelGoal(args.ID, args.Goal);
        }
        private void OnMoveCountChangeHandler(int count)
        {
            _view.SetMoveText(count);
        }
        private void AddEvents()
        {
            foreach (var goal in _conditionsProvider.LevelGoals)
            {
                goal.OnGoalChanged += OnGoalChangedHandler;
            }
            _conditionsProvider.MoveCount.ValueChanged += OnMoveCountChangeHandler;
        }
        private void RemoveEvents()
        {
            foreach (var goal in _conditionsProvider.LevelGoals)
            {
                goal.OnGoalChanged -= OnGoalChangedHandler;
            }
            _conditionsProvider.MoveCount.ValueChanged -= OnMoveCountChangeHandler;
        }
    }
}

