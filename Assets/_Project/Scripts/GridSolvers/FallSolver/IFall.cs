using Cysharp.Threading.Tasks;
using GridSystem;
using Item;
using Level;

namespace App
{
    public interface IFall
    {
        UniTask Fall(IGameBoard gameBoard,IItemFactory itemFactory,ItemsContainer container, LevelData levelData);
        UniTask Fall(IGridSlot slot, IGameBoard gameBoard, IItemFactory itemFactory, ItemsContainer container, LevelData levelData);
    }
}

