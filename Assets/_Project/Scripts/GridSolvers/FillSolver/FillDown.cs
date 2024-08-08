using Cysharp.Threading.Tasks;
using DG.Tweening;
using Extensions;
using GridSystem;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
                    item.IsMoving = true;
                    gridSlot.Clear();
                    gameBoard[targetPos].SetItem(item);
                    var distance = gridSlot.GridPosition.y - targetPos.y;

                    _ = seq.Join(item.FallTween(gameBoard.GridToWorldCenter(targetPos), distance,
                        onComplete: () =>
                        {
                            item.SetOrder(targetPos.y);
                            item.IsMoving = false;
                        }));
                }
            }
            await seq;
        }
        public UniTask Fill(IGameBoard gameBoard, IGridSlot slot)
        {
            var seq = DOTween.Sequence();
            for (var i = 0; i < gameBoard.Height; i++)
            {
                var gridSlot = gameBoard[slot.GridPosition.x, i];
                if (!gridSlot.HasItem)
                {
                    continue;
                }
                if (!CanDropDown(gameBoard, gridSlot, out var targetPos))
                {
                    continue;
                }
                var item = gridSlot.Item;
                item.IsMoving = true;
                gridSlot.Clear();
                gameBoard[targetPos].SetItem(item);
                var distance = gridSlot.GridPosition.y - targetPos.y;

                _ = seq.Join(item.FallTween(gameBoard.GridToWorldCenter(targetPos), distance,
                    onComplete: () =>
                    {
                        item.IsMoving = false;
                        item.SetOrder(targetPos.y);
                    }));
            }
            return seq.ToUniTask();
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

