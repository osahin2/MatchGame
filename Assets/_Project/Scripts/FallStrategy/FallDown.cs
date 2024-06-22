using Cysharp.Threading.Tasks;
using DG.Tweening;
using GridSystem;
using Item;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace App
{
    public class FallDown : IFall
    {
        public async UniTask Fall(IGameBoard gameBoard, IItemFactory itemFactory, ItemsContainer container)
        {
            var fallSeq = DOTween.Sequence();

            var emptySlots = FindEmptySlots(gameBoard);

            foreach ( var slot in emptySlots)
            {
                var item = itemFactory.Get(ItemType.MatchItem);
                var topGrid = gameBoard.GetGridSlot(new Vector2Int(slot.GridPosition.x, gameBoard.Height - 1));
                item.SetPosition(gameBoard.GridToWorldCenter(topGrid.GridPosition) + new Vector3(0f, 1f));
                item.SetItemInfo(container.Items[Random.Range(0, 4)]);
                item.SetOrder(slot.GridPosition.y);
                item.Show();
                slot.SetItem(item);
                _ = fallSeq.Join(item.Transform
                    .DOMove(gameBoard.GridToWorldCenter(slot.GridPosition), .35f)
                    .SetEase(Ease.OutBounce));
            }

            await fallSeq;
        }
        private List<IGridSlot> FindEmptySlots(IGameBoard gameBoard)
        {
            var emptySlots = gameBoard.GridSlots1D.Where(x => !x.HasItem).ToList();
            return emptySlots;
        }
    }
}

