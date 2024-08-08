using GridSystem;
using System.Collections.Generic;

namespace App
{
    public interface IGridSolver
    {
        void SolveMatches();
        void Solve(IGridSlot slot);
        void Solve(IEnumerable<IGridSlot> slots);
        void SolveColumn(IGridSlot slot);
        bool HasMatch(IGridSlot slot);
        void SingleGridHit(IGridSlot slot);
    }

}

