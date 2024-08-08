using Cysharp.Threading.Tasks;
using GridSystem;
using System.Collections.Generic;
using UnityEngine;

namespace App
{
    public interface IPowerSolver
    {
        UniTask Solve(Vector2Int pos, IEnumerable<IGridSlot> matches, out IEnumerable<IGridSlot> solved);
    }

}

