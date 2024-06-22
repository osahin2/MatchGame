using GridSystem;
using Service_Locator;
using System;
using UnityEngine;
namespace Item
{
    public class Duck : SpecialItemBase
    {
        private IGameBoard GameBoard => _gameBoard ??= ServiceProvider.Instance.Get<IGameBoard>();
        private IGameBoard _gameBoard; 
        public override void CheckSpecial(Action onSuccess)
        {
            var gridPos = GameBoard.WorldToGrid(transform.position);
            Debug.Log(gridPos);
            if (gridPos.y == 0)
            {
                onSuccess?.Invoke();
            }
        }

        public override void Interact(Action onSuccess)
        {
            Debug.Log("Duck Intereact");
        }
    }
}