using Cysharp.Threading.Tasks;
using DG.Tweening;
using GridSystem;
using Inputs;
using Item;
using Level;
using Power;
using Service_Locator;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;

namespace App
{
    public class Gameplay : MonoBehaviour
    {
        public event Action<bool> OnLevelFinished;

        [SerializeField] private GameBoard _gameBoard;
        [SerializeField] private List<GenericItemFactory> _itemFactories;
        [SerializeField] private List<GenericPowerItemFactory> _powerFactories;
        [SerializeField] private ItemsContainer _itemDataContainer;
        [SerializeField] private PowerConditionContainer _powerConditions;

        private IItemFactory _itemFactory;
        private IInputSystem _inputSystem;
        private IGridSolver _gridSolver;
        private ILevel _level;
        private ILevelConditionsProvider _levelConditionsProvider;
        private LevelData _levelData;


        public void Construct(IInputSystem inputSystem, ILevel level)
        {
            _inputSystem = inputSystem;
            _level = level;

            _powerConditions.Construct();
            _itemDataContainer.Construct();

            _itemFactory = new RecyclableItemFactory(_itemFactories, _powerFactories);
            _gridSolver = new GridSolver(_gameBoard, _itemDataContainer, _itemFactory, _powerConditions);

            ServiceProvider.Instance
                .Register<IGameBoard>(_gameBoard)
                .Register(_gridSolver)
                .Register(_itemFactory)
                .Get(out _levelConditionsProvider);
        }
        public async void Init()
        {
            _levelData = _level.GetLevelData();
            _gameBoard.Init(_levelData.GridWidth, _levelData.GridHeigth);

            await FillBoardAsync();
            _gridSolver.SolveMatches();
            _inputSystem.SetActive(true);
            _inputSystem.SetInputActive(true);
            AddEvents();
        }
        public void DeInit()
        {
            RemoveEvents();

            ClearItems();
            _gameBoard.DeInit();
        }
        //TODO: Create SimpleFill class and fill on start with that
        private async UniTask FillBoardAsync()
        {
            var seq = DOTween.Sequence();
            var itemsToShow = new List<IItem>();
            for (int xCoor = 0; xCoor < _gameBoard.Width; xCoor++)
            {
                for (int yCoor = 0; yCoor < _gameBoard.Height; yCoor++)
                {
                    var slot = _gameBoard[xCoor, yCoor];
                    var gridData = _levelData.GetGridData(slot.GridPosition);
                    var item = _itemFactory.GetItem(gridData.itemData.ItemType);
                    item.SetPosition(_gameBoard.GridToWorldCenter(slot.GridPosition));
                    item.SetItemInfo(gridData.itemData);
                    item.SetOrder(yCoor);
                    slot.SetItem(item);
                    itemsToShow.Add(item);
                }
            }
            foreach (var item in itemsToShow)
            {
                item.SetScale(0f);
                item.Show();
                _ = seq.Join(item.Transform.DOScale(1f, .25f).SetEase(Ease.OutBack));
            }
            await seq;
        }
        private bool CheckGoals()
        {
            foreach (var goal in _levelConditionsProvider.LevelGoals)
            {
                if (!goal.IsAchieved)
                    return false;
            }
            return true;
        }
        private void ClearItems()
        {
            foreach (var slot in _gameBoard.GridSlots1D)
            {
                slot.Item.Hide();
                _itemFactory.FreeItem(slot.Item);
            }
        }
        private void OnPointerDownHandler(object sender, InputEventArgs args)
        {
            var pos = _gameBoard.WorldToGrid(args.WorldPosition);
            if (!GridUtility.IsPositionOnGrid(pos, _gameBoard.Width, _gameBoard.Height))
                return;
            var slot = _gameBoard[pos];

            if (!slot.HasItem)
                return;

            slot.Item.Interact();

        }
        private void OnMoveReachedZeroHandler()
        {
            OnLevelFinished?.Invoke(false);
        }
        private void OnGoalChangeHandler(object sender, LevelGoalEventArgs args)
        {
            if (CheckGoals())
            {
                OnLevelFinished?.Invoke(true);
            }
        }
        private void AddEvents()
        {
            _inputSystem.PointerDown += OnPointerDownHandler;
            _levelConditionsProvider.OnMoveReachedZero += OnMoveReachedZeroHandler;
            foreach (var goal in _levelConditionsProvider.LevelGoals)
            {
                goal.OnGoalChanged += OnGoalChangeHandler;
            }


        }
        private void RemoveEvents()
        {
            _inputSystem.PointerDown -= OnPointerDownHandler;
            _levelConditionsProvider.OnMoveReachedZero -= OnMoveReachedZeroHandler;
            foreach (var goal in _levelConditionsProvider.LevelGoals)
            {
                goal.OnGoalChanged -= OnGoalChangeHandler;
            }
        }
    }

}

