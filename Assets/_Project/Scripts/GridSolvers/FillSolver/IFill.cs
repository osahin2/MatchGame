using Cysharp.Threading.Tasks;
using GridSystem;
using System.Collections.Generic;
using System.Threading;

namespace App
{
    public interface IFill
    {
        UniTask Fill(IGameBoard gameBoard, IEnumerable<IGridSlot> solvedGrids);
        UniTask Fill(IGameBoard gameBoard, IGridSlot slot);
    }
}

