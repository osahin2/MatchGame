using Cysharp.Threading.Tasks;
using DG.Tweening;
using GridSystem;
using Item;
using Power;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace App
{
    public class PowerSolver : IPowerSolver
    {
        private PowerConditionContainer _container;
        private IItemFactory _itemFactory;
        private IGameBoard _gameBoard;
        public PowerSolver(IGameBoard gameBoard,PowerConditionContainer container, IItemFactory itemFactory)
        {
            _gameBoard = gameBoard;
            _container = container;
            _itemFactory = itemFactory;
        }
        public UniTask Solve(Vector2Int pos,IEnumerable<IGridSlot> matches, out IEnumerable<IGridSlot> solved)
        {
            solved = null;
            if(CheckPowerCondition(matches, out var powerMatches, out var powerData))
            {
                var seq = DOTween.Sequence();

                foreach (var slot in powerMatches)
                {
                    _ = seq.Join(slot.Item.Transform.
                        DOMove(_gameBoard[pos].Item.Transform.position, .25f)
                        .SetEase(Ease.InBack));
                }

                _gameBoard[pos].Item.Hide();
                _itemFactory.FreeItem(_gameBoard[pos].Item);
                _gameBoard[pos].Clear();

                var item = _itemFactory.GetPowerItem(powerData.PowerType);
                item.SetPosition(_gameBoard.GridToWorldCenter(pos));
                item.SetItemInfo(powerData.ItemData);
                item.SetOrder(pos.y);
                item.Show();

                _gameBoard[pos].SetItem(item);
                solved = matches.Where(x => x.GridPosition != pos).ToList();

                return seq.ToUniTask();
            }
            return UniTask.Yield().ToUniTask();
        }
        private bool CheckPowerCondition(IEnumerable<IGridSlot> matches, 
            out List<IGridSlot> powerMatches, out PowerItemData powerData)
        {
            var matchItems = matches.Where(x => x.Item.IsMatchItem).ToList();
            if (_container.TryGetPower(matchItems.Count, out var power))
            {
                powerData = power;
                powerMatches = matchItems;
                return true;
            }
            powerData = null;
            powerMatches = null;
            return false;
        }
    }

}

