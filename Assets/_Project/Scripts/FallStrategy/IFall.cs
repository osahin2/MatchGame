using Cysharp.Threading.Tasks;
using GridSystem;
using Item;

namespace App
{
    public interface IFall
    {
        UniTask Fall(IGameBoard gameBoard,IItemFactory itemFactory,ItemsContainer container);
    }
}

