using Cysharp.Threading.Tasks;
using DG.Tweening;
using Extensions;
using GridSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace App
{
    public class FillDown : IFill
    {
        public async UniTask Fill(IGameBoard gameBoard, IEnumerable<IGridSlot> solvedGrids)
        {
            var seq = DOTween.Sequence();
            foreach(var grid in solvedGrids)
            {
                for (var i = 0; i < gameBoard.Height; i++)
                {
                    var gridSlot = gameBoard[grid.GridPosition.x, i];
                    if (!gridSlot.HasItem)
                    {
                        continue;
                    }
                    if(!CanDropDown(gameBoard, gridSlot, out var targetPos))
                    {
                        continue;
                    }
                    var item = gridSlot.Item;
                    gridSlot.Clear();
                    _ = seq.Join(item.Transform.
                        DOMove(gameBoard.GridToWorldCenter(targetPos), .35f)
                        .SetEase(Ease.OutBounce)
                        .OnComplete(() =>
                        {
                            item.SetOrder(targetPos.y);
                        }));
                    gameBoard[targetPos].SetItem(item);
                }
            }
            await seq;
        }
        private bool CanDropDown(IGameBoard gameBoard, IGridSlot slot, out Vector2Int targetPos)
        {
            var targetGridSlot = slot;
            while (IsMoveable(gameBoard, targetGridSlot, out var gridPos))
            {
                targetGridSlot = gameBoard[gridPos];
            }
            targetPos = targetGridSlot.GridPosition;
            return targetGridSlot != slot;
        }
        private bool IsMoveable(IGameBoard gameBoard, IGridSlot slot, out Vector2Int pos)
        {
            var targetGrid = gameBoard.GetGridSlot(slot.GridPosition.Down());

            if(targetGrid == null || targetGrid.HasItem) 
            {
                pos = Vector2Int.zero;
                return false;
            }
            pos = targetGrid.GridPosition;
            return true;
        }
    }
}

