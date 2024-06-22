using Cysharp.Threading.Tasks;
using DG.Tweening;
using GridSystem;
using Inputs;
using Item;
using Service_Locator;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

namespace App
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private List<>
    }
    public class Gameplay : MonoBehaviour
    {
        [SerializeField] private GameBoard _gameBoard;
        [SerializeField] private List<GenericItemFactory> _itemFactories;
        [SerializeField] private ItemsContainer _itemDataContainer;

        private IItemFactory _itemFactory;
        private IInputSystem _inputSystem;
        private GridMatchSolver _matchSolver;
        private IFill _fillSolver;
        private IFall _fallSolver;

        private List<IItem> _itemsToShow = new();

        public void Construct(IInputSystem inputSystem)
        {
            ServiceProvider.Instance.Register<IGameBoard>(_gameBoard);

            _inputSystem = inputSystem;

            _itemDataContainer.Construct();
            _gameBoard.Construct();
            ConstructFactory();
            _matchSolver = new GridMatchSolver(_gameBoard);
            _fillSolver = new FillDown();
            _fallSolver = new FallDown();
            Init();
        }
        private void ConstructFactory()
        {
            _itemFactory = new RecyclableItemFactory(_itemFactories);
        }
        public async void Init()
        {
            await FillBoardAsync();
            _matchSolver.Solve();
            AddEvents();
        }
        public void DeInit()
        {
            RemoveEvents();
        }
        //TODO: Create SimpleFill class and fill on start with that
        private async UniTask FillBoardAsync()
        {
            var seq = DOTween.Sequence();

            for (int xCoor = 0; xCoor < _gameBoard.Width; xCoor++)
            {
                for (int yCoor = 0; yCoor < _gameBoard.Height; yCoor++)
                {
                    var slot = _gameBoard[xCoor, yCoor];
                    int random = Random.Range(0, 4);
                    var item = _itemFactory.Get((ItemType)random);
                    item.SetPosition(_gameBoard.GridToWorldCenter(slot.GridPosition));
                    item.SetItemInfo(_itemDataContainer.GetItemData((ItemType)random));
                    item.SetOrder(yCoor);
                    slot.SetItem(item);
                    _itemsToShow.Add(item);
                }
            }
            foreach (var item in _itemsToShow)
            {
                item.Show();
                _ = seq.Join(item.Transform.DOScale(1f, .25f).SetEase(Ease.OutBack));
            }
            await seq;
        }
        private void OnPointerDownHandler(object sender, InputEventArgs args)
        {
            var pos = _gameBoard.WorldToGrid(args.WorldPosition);
            if (!GridUtility.IsPositionOnGrid(pos, _gameBoard.Width, _gameBoard.Height))
                return;

            if (!_matchSolver.TryFindSolvedMatch(_gameBoard[pos], out var match))
            {
                return;
            }
            _gameBoard[pos].Item.Interact(() =>
            {
                SolveJobs(match);
            });
        }
        private async void SolveJobs(IEnumerable<IGridSlot> match)
        {
            foreach (var slot in match)
            {

                slot.Item.Hide();
                _itemFactory.Free(slot.Item);
                slot.Clear();
            }
            await _fillSolver.Fill(_gameBoard, match);
            await _fallSolver.Fall(_gameBoard, _itemFactory, _itemDataContainer);
            _matchSolver.Solve();
            CheckSpecialItems();
        }
        private void CheckSpecialItems()
        {
            var conditionedSpecials = new List<IGridSlot>();
            foreach (var item in _gameBoard.GridSlots1D)
            {
                if (item.Item is SpecialItemBase specialItem)
                {
                    specialItem.CheckSpecial(() =>
                    {
                        conditionedSpecials.Add(item);
                    });
                }
            }
            if (conditionedSpecials.Count == 0)
                return;

            SolveJobs(conditionedSpecials);
        }
        private void AddEvents()
        {
            _inputSystem.PointerDown += OnPointerDownHandler;
        }
        private void RemoveEvents()
        {
            _inputSystem.PointerDown -= OnPointerDownHandler;
        }
    }
    
}

