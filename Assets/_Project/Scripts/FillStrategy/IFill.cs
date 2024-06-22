using Cysharp.Threading.Tasks;
using GridSystem;
using System.Collections.Generic;

namespace App
{
    public interface IFill
    {
        UniTask Fill(IGameBoard gameBoard, IEnumerable<IGridSlot> solvedGrids);
    }
}

