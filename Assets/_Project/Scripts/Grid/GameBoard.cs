using System.Collections.Generic;
using UnityEngine;
using Utilities;
namespace GridSystem
{
    public class GameBoard : MonoBehaviour, IGameBoard
    {
        [Header("Grid Settings")]
        [SerializeField] private int _gridWidth;
        [SerializeField] private int _gridHeight;
        [SerializeField] private float _slotSize;
        [Header("Grid Border")]
        [SerializeField] private SpriteRenderer _gridBorder;
        [SerializeField] private Vector2 _defaultSize;

        private IGridSlot[,] _gridSlots;

        public int GridSize => _gridWidth * _gridHeight;
        public int Width => _gridWidth;
        public int Height => _gridHeight;
        public IGridSlot[,] GetSlots => _gridSlots;
        public IGridSlot this[int row, int column] => _gridSlots[row, column];
        public IGridSlot this[Vector2Int position] => _gridSlots[position.x, position.y];
        public List<IGridSlot> GridSlots1D { get; private set; } = new();

        private Vector3 _origin;

        public void Init(int width = 0, int heigth = 0)
        {
            _gridWidth = width == 0 ? _gridWidth : width;
            _gridHeight = heigth == 0 ? _gridHeight : heigth;

            _gridSlots = new IGridSlot[_gridWidth, _gridHeight];
            GridSlots1D = new List<IGridSlot>();
            CreateGrid();
            AdjustBorderToGrid();
        }
        public void DeInit()
        {
            _gridBorder.size = _defaultSize;
            _gridSlots = null;
            GridSlots1D.Clear();
        }
        private void CreateGrid()
        {
            var originX = _gridWidth / 2f * _slotSize;
            var originY = _gridHeight / 2f * _slotSize;

            _origin = Vector3.zero - new Vector3(originX, originY);

            for (int x = 0; x < _gridWidth; x++)
            {
                for (int y = 0; y < _gridHeight; y++)
                {
                    var gridSlot = new GridSlot(new Vector2Int(x, y));
                    _gridSlots[x, y] = gridSlot;
                    GridSlots1D.Add(gridSlot);
                }
            }
        }
        private void AdjustBorderToGrid()
        {
            _gridBorder.size += new Vector2(_gridWidth / 2f - .5f, _gridHeight / 2f - .5f);
        }
        public Vector3 GridToWorld(Vector2Int gridPosition)
        {
            return (new Vector3(gridPosition.x, gridPosition.y) * _slotSize) + _origin;
        }
        public Vector3 GridToWorld(int x, int y)
        {
            return (new Vector3(x, y) * _slotSize) + _origin;
        }
        public Vector3 GridToWorldCenter(Vector2Int gridPosition)
        {
            return GridToWorld(gridPosition) + new Vector3(_slotSize, _slotSize) * .5f;
        }
        public Vector2Int WorldToGrid(Vector3 worldPosition)
        {
            var x = Mathf.FloorToInt((worldPosition - _origin).x / _slotSize);
            var y = Mathf.FloorToInt((worldPosition - _origin).y / _slotSize);

            return new Vector2Int(x, y);
        }
        public IGridSlot GetGridSlot(Vector2Int position)
        {
            return GridUtility.IsPositionOnGrid(position, _gridHeight, _gridWidth) ? this[position] : null;
        }
    }
}
