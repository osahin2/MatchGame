using Extensions;
using GridSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace App
{
    public class GridMatchSolver
    {
        private IGameBoard _gameBoard;

        private readonly int _row;
        private readonly int _col;
        private int _matchIndex = -1;

        private HashSet<IGridSlot>[] _matches;
        private readonly Dictionary<Vector2Int, int> _lookupDict = new();

        private bool[,] _visited;

        public GridMatchSolver(IGameBoard gameBoard)
        {
            _gameBoard = gameBoard;
            _row = gameBoard.Width;
            _col = gameBoard.Height;
        }
        public void Solve()
        {
            _matches = new HashSet<IGridSlot>[_row * _col];            
            _lookupDict.Clear();
            _matchIndex = -1;

            SolveMatchItems();
            SolveSpecialItems();
        }
        private void SolveMatchItemsDFS()
        {
            //_visited = new bool[_row, _col];
            //Stack<Vector2Int> stack = new();

            //var startSlot = Vector2Int.zero;

            //_visited[startSlot.x, startSlot.y] = true;
            //stack.Push(startSlot);

            //while (stack.Count != 0)
            //{
            //    var slot = stack.Pop();
            //    var directions = GetDirections(slot);

            //    //_visited[slot.x, slot.y] = true;

            //    foreach (var direction in directions)
            //    {
            //        if (IsOutOfGrid(direction))
            //            continue;

            //        if (IsVisited(direction))
            //        {
            //            CheckIfConnectedAndAddMatch(direction, slot);
            //            continue;
            //        }

            //        _visited[direction.x, direction.y] = true;
            //        stack.Push(direction);

            //        CheckIfConnectedAndAddMatch(slot, direction);
            //    }
            //}
        }
        private void SolveMatchItems()
        {
            foreach (var slot in _gameBoard.GridSlots1D)
            {
                var directions = GetDirections(slot.GridPosition);
                foreach (var direction in directions)
                {
                    if (IsOutOfGrid(direction))
                        continue;
                    CheckIfConnectedAndAddMatch(slot.GridPosition, direction);
                }
            }
        }
        private void SolveSpecialItems()
        {
            var specialPoppableGrids = _gameBoard.GridSlots1D.
                Where(x => !x.Item.IsMatchItem && x.Item.IsPoppable).ToList();
            foreach (var grid in specialPoppableGrids)
            {
                var directions = GetDirections(grid.GridPosition);

                foreach (var dir in directions)
                {
                    if(IsOutOfGrid(dir)) 
                        continue;
                    if (!_gameBoard[dir].Item.IsMatchItem)
                        continue;
                    if(TryFindMatchIndex(dir, out var index))
                    {
                        AddMatch(grid.GridPosition, dir, index);
                    }
                }
            }
        }
        public bool TryFindSolvedMatch(IGridSlot current, out IEnumerable<IGridSlot> matches)
        {
            if (TryFindMatchIndex(current.GridPosition, out var index))
            {
                matches = _matches[index];
                return true;
            }
            matches = null;
            return false;
        }
        private void CheckIfConnectedAndAddMatch(Vector2Int current, Vector2Int next)
        {
            if (IsConnected(current, next))
            {
                int currentIndex;
                int nextIndex;

                if (TryFindMatchIndex(current, out currentIndex)
                    && TryFindMatchIndex(next, out nextIndex))
                {
                    _matches[currentIndex].UnionWith(_matches[nextIndex]);
                    _lookupDict[next] = currentIndex;
                }
                else if (TryFindMatchIndex(current, out currentIndex))
                {
                    AddMatch(current, next, currentIndex);
                }
                else if (TryFindMatchIndex(next, out nextIndex))
                {
                    AddMatch(next, current, nextIndex);
                }
                else
                {
                    _matchIndex++;
                    AddMatch(current, next, _matchIndex);
                }
            }
        }
        private bool TryFindMatchIndex(Vector2Int slot, out int index)
        {
            if (_lookupDict.TryGetValue(slot, out index))
            {
                return true;
            }
            return false;
        }
        private void AddMatch(Vector2Int current, Vector2Int next, int index)
        {
            if (_matches[index] == null)
            {
                _matches[index] = new HashSet<IGridSlot>();
            }
            if (!_matches[index].Contains(_gameBoard[current]))
            {
                _matches[index].Add(_gameBoard[current]);

                if (!_lookupDict.ContainsKey(current))
                {
                    _lookupDict.Add(current, index);
                }
            }
            _matches[index].Add(_gameBoard[next]);

            if (!_lookupDict.ContainsKey(next))
            {
                _lookupDict.Add(next, index);
            }
        }
        private bool IsConnected(Vector2Int current, Vector2Int neighbour)
        {
            if (!_gameBoard[current].Item.IsMatchItem || !_gameBoard[neighbour].Item.IsMatchItem)
                return false;

            if (_gameBoard[current].Item.ItemID == _gameBoard[neighbour].Item.ItemID)
            {
                return true;
            }
            return false;
        }
        private bool IsOutOfGrid(Vector2Int slot)
        {
            return slot.x < 0 || slot.x >= _row || slot.y < 0 || slot.y >= _col;
        }
        private bool IsVisited(Vector2Int slot)
        {
            return _visited[slot.x, slot.y];
        }
        private List<Vector2Int> GetDirections(Vector2Int slot)
        {
            return new() { slot.Left(), slot.Right(), slot.Up(), slot.Down() };
        }
    }
}

