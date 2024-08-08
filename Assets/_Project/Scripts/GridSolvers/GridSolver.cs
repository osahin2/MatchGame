using Cysharp.Threading.Tasks;
using DG.Tweening;
using GridSystem;
using Inputs;
using Item;
using Level;
using Power;
using Service_Locator;
using System.Collections.Generic;
using System.Linq;

namespace App
{
    public class GridSolver : IGridSolver
    {
        private GridMatchSolver _matchSolver;
        private IFill _fillSolver;
        private IFall _fallSolver;
        private IPowerSolver _powerSolver;
        private IGameBoard _gameBoard;
        private IItemFactory _itemFactory;
        private ItemsContainer _itemDataContainer;
        private LevelData _levelData => Level.GetLevelData();

        private ILevelConditionsProvider Conditions => _conditions
            ??= ServiceProvider.Instance.Get<ILevelConditionsProvider>();

        private ILevelConditionsProvider _conditions;

        private IInputSystem Input => _input ??= ServiceProvider.Instance.Get<IInputSystem>();
        private IInputSystem _input;

        private ILevel Level => _level ??= ServiceProvider.Instance.Get<ILevel>();
        private ILevel _level;

        public GridSolver(IGameBoard gameBoard, ItemsContainer container, IItemFactory itemFactory,
            PowerConditionContainer conditions)
        {
            _itemFactory = itemFactory;
            _gameBoard = gameBoard;
            _itemDataContainer = container;

            _matchSolver = new GridMatchSolver(gameBoard);
            _fillSolver = new FillDown(); 
            _fallSolver = new FallDown();
            _powerSolver = new PowerSolver(_gameBoard, conditions, _itemFactory);
        }
        public async void Solve(IGridSlot slot)
        {
            if (_matchSolver.TryFindSolvedMatch(slot, out var matches))
            {
                Input.SetInputActive(false);
                await _powerSolver.Solve(slot.GridPosition, matches, out var powerMatches);

                var isPowerMatch = powerMatches != null;
                if (isPowerMatch)
                {
                    matches = powerMatches;
                }
                matches = matches.Where(x => !x.Item.IsMoving).ToList();
                ClearItems(matches, isPowerMatch);

                Solve(matches);
            }
        }

        public void SingleGridHit(IGridSlot slot)
        {
            if (!slot.HasItem)
                return;

            if (slot.Item.IsPower)
            {
                slot.Item.Pop();
                return;
            }

            if (!slot.Item.IsPoppable)
                return;

            var item = slot.Item;
            slot.Clear();
            if (item.IsMatchItem)
            {
                if (Conditions.TryGetGoal(item.ItemID, out var goal))
                {
                    item.Pop();
                    item.UIMove(onComplete: () =>
                    {
                        goal.DecreaseGoal();
                        item.Hide();
                        _itemFactory.FreeItem(item);
                    });
                    return;
                }
            }
            item.Pop();
            item.Hide();
            _itemFactory.FreeItem(item);
        }
        public async void SolveColumn(IGridSlot slot)
        {
            var fill = _fillSolver.Fill(_gameBoard, slot);
            var fall = _fallSolver.Fall(slot, _gameBoard, _itemFactory, _itemDataContainer, _levelData);
            await UniTask.WhenAll(fill, fall);
            CheckSpecialItems();
            _matchSolver.Solve();
        }
        public async void Solve(IEnumerable<IGridSlot> slots)
        {
            _matchSolver.Solve();
            var fillTask = _fillSolver.Fill(_gameBoard, slots);
            var fallTask = _fallSolver.Fall(_gameBoard, _itemFactory, _itemDataContainer, _levelData);
            _matchSolver.Solve();
            await UniTask.WhenAll(fillTask, fallTask);
            Input.SetInputActive(true);
            CheckSpecialItems();
        }

        public void SolveMatches()
        {
            _matchSolver.Solve();
        }
        public bool HasMatch(IGridSlot slot)
        {
            return _matchSolver.HasAnyMatch(slot.GridPosition);
        }
        private void ClearItems(IEnumerable<IGridSlot> matches, bool isPowerMatch)
        {
            var moveSeq = DOTween.Sequence();

            foreach (var match in matches)
            {
                var item = match.Item;
                match.Clear();
                if (Conditions.TryGetGoal(item.ItemID, out var goal))
                {
                    if (isPowerMatch)
                    {
                        goal.DecreaseGoal();
                        item.Hide();
                        _itemFactory.FreeItem(item);
                        continue;
                    }
                    item.Pop();
                    item.SetOrder(SortOrders.UI_MOVE);
                    _ = moveSeq.Join(item.UIMove(onComplete: () =>
                    {
                        goal.DecreaseGoal();
                        item.Hide();
                        _itemFactory.FreeItem(item);
                    })).PrependInterval(.05f);
                }
                else
                {
                    if (!isPowerMatch)
                        item.Pop();

                    item.Hide();
                    _itemFactory.FreeItem(item);
                }
            }

            moveSeq.Play();
        }
        private void CheckSpecialItems()
        {
            var conditionSpecials = new List<IGridSlot>();
            foreach (var item in _gameBoard.GridSlots1D)
            {
                if (item.Item is SpecialItemBase specialItem)
                {
                    specialItem.CheckSpecial(() =>
                    {
                        conditionSpecials.Add(item);
                    });
                }
            }
            if (conditionSpecials.Count == 0)
                return;

            ClearItems(conditionSpecials, false);
            Solve(conditionSpecials);
        }

    }

}

