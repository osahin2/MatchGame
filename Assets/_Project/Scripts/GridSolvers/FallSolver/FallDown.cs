using Cysharp.Threading.Tasks;
using DG.Tweening;
using GridSystem;
using Item;
using Level;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace App
{
    public class FallDown : IFall
    {
        public async UniTask Fall(IGameBoard gameBoard, IItemFactory itemFactory, ItemsContainer container, LevelData levelData)
        {

            var emptySlots = FindEmptySlots(gameBoard);
            SpawnItemsToTargetPositions(gameBoard, itemFactory, levelData, container, emptySlots);
            var fallSeq = CreateFallSequence(gameBoard, emptySlots);

            await fallSeq;
        }
        public UniTask Fall(IGridSlot slot, IGameBoard gameBoard, IItemFactory itemFactory, ItemsContainer container, LevelData levelData)
        {
            var emptySlots = FindEmptySlotsOnColumn(gameBoard, slot);
            SpawnItemsToTargetPositions(gameBoard, itemFactory, levelData, container, emptySlots);
            var fallSeq = CreateFallSequence(gameBoard, emptySlots);
            return fallSeq.ToUniTask();
        }
        private Sequence CreateFallSequence(IGameBoard gameBoard, List<IGridSlot> emptySlots)
        {
            var fallSeq = DOTween.Sequence();

            foreach (var slot in emptySlots)
            {
                var itemPosition = gameBoard.WorldToGrid(slot.Item.Transform.position);
                var topGrid = gameBoard.GetGridSlot(new Vector2Int(slot.GridPosition.x, gameBoard.Height - 1));

                var spawnPointDistance = itemPosition.y - topGrid.GridPosition.y;
                var distance = (topGrid.GridPosition.y + spawnPointDistance) - slot.GridPosition.y;
                slot.Item.IsMoving = true;

                _ = fallSeq.Join(slot.Item.FallTween(gameBoard.GridToWorldCenter(slot.GridPosition), distance,
                    onComplete: () =>
                    {
                        slot.Item.IsMoving = false;
                        slot.Item.SetOrder(slot.GridPosition.y);
                    }))
                    .PrependInterval(.05f);
            }
            return fallSeq;
        }
        private void SpawnItemsToTargetPositions(IGameBoard gameBoard, IItemFactory itemFactory,
            LevelData levelData, ItemsContainer container, List<IGridSlot> emptySlots)
        {
            var spawnedXPos = new Dictionary<int, List<IItem>>();

            foreach (var slot in emptySlots)
            {
                var randomFallItem = levelData.FallItems[Random.Range(0, levelData.FallItems.Count)];

                var item = itemFactory.GetItem(randomFallItem.ItemType);
                var itemData = container.GetItemDataById(randomFallItem.ID);
                var topGrid = gameBoard.GetGridSlot(new Vector2Int(slot.GridPosition.x, gameBoard.Height - 1));
                var itemSpawnPos = gameBoard.GridToWorldCenter(topGrid.GridPosition) + new Vector3(0f, 2f);

                item.SetPosition(itemSpawnPos);
                item.SetItemInfo(itemData);
                item.SetOrder(SortOrders.ITEM_SPAWN);
                item.Show();
                slot.SetItem(item);

                //Add dictionary every new column with item and check for all same column items,
                //if there are items push them up one grid 
                if (spawnedXPos.TryGetValue(slot.GridPosition.x, out var items))
                {
                    foreach (var item1 in items)
                    {
                        item1.SetPosition(item1.Transform.position + Vector3.up / 2f);
                    }
                    spawnedXPos[slot.GridPosition.x].Add(item);
                }
                else
                {
                    spawnedXPos.Add(slot.GridPosition.x, new List<IItem> { item });
                }


            }
        }
        private List<IGridSlot> FindEmptySlotsOnColumn(IGameBoard gameBoard, IGridSlot slot)
        {
            var emptySlots = new List<IGridSlot>();
            for (int i = gameBoard.Height - 1; i >= 0; i--)
            {
                var targetSlot = gameBoard[slot.GridPosition.x, i];
                if (!targetSlot.HasItem)
                    emptySlots.Add(targetSlot);
            }
            return emptySlots;
        }
        private List<IGridSlot> FindEmptySlots(IGameBoard gameBoard)
        {
            var emptySlots = gameBoard.GridSlots1D.Where(x => !x.HasItem).OrderByDescending(x => x.GridPosition.y).ToList();
            return emptySlots;
        }
    }
}

