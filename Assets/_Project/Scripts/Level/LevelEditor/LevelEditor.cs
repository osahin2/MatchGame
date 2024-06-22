#if UNITY_EDITOR

using GridSystem;
using Item;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace Level
{
    public class LevelEditor : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        [Space, Header("Level Data")]
        [SerializeField] private LevelData _editLevelData;
        [SerializeField] private Button _levelDataCreateButton;
        [SerializeField] private TMP_InputField _levelName;
        [SerializeField] private string _dataPath;
        [Space, Header("Level Goal")]
        [SerializeField] private TMP_Dropdown _levelGoalsDropdown;
        [SerializeField] private TMP_InputField _levelGoal;
        [SerializeField] private Button _addGoal;
        [SerializeField] private Button _clearGoals;

        [Space, Header("Grid")]
        [SerializeField] private GameBoard _gameBoard;
        [SerializeField] private TMP_InputField _widthInput;
        [SerializeField] private TMP_InputField _heightInput;
        [SerializeField] private Button _createGrid;

        [Space, Header("Items")]
        [SerializeField] private ItemsContainer _itemContainer;
        [SerializeField] private Transform _selectItemParent;
        [SerializeField] private LevelEditorItemButton _buttonPrefab;
        [SerializeField] private LevelEditorGridItem _gridItem;

        [Space, Header("Fall Items")]
        [SerializeField] private Button _addFallItemsButton;
        [SerializeField] private Button _clearFallItems;
        [SerializeField] private Transform _addedFallItemsContainer;



        private List<LevelGoal> _addedGoals = new();
        private List<LevelEditorItemButton> _itemButtons = new();
        private LevelEditorItemButton _selectedButton;
        private List<LevelEditorGridItem> _spawnedItems = new();
        private List<LevelEditorItemButton> _spawnedFallItems = new();
        private List<int> _fallItems = new();

        private int _width;
        private int _height;

        private void Awake()
        {
            _itemContainer.Construct();
            AddOptionsToGoalDropdown();
            CreateEditorButtons();

            _addGoal.onClick.AddListener(OnAddGoalClicked);
            _createGrid.onClick.AddListener(OnCreateGridClicked);
            _levelDataCreateButton.onClick.AddListener(OnLevelDataCreateClicked);
            _clearGoals.onClick.AddListener(OnClearGoalsClicked);
            _addFallItemsButton.onClick.AddListener(OnAddFallItemsClicked);
            _clearFallItems.onClick.AddListener(OnClearFallItemsClicked);

            foreach (var item in _itemButtons)
            {
                item.AddListener(OnItemButtonClicked);
            }
        }
        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                if (_selectedButton != null)
                {
                    PutItemsToGrid();
                }
            }
            if(Input.GetMouseButtonDown(1))
            {
                if(_selectedButton != null)
                {
                    _selectedButton.DeSelected();
                }
            }
        }
        private void PutItemsToGrid()
        {
            var worldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
            var gridPos = _gameBoard.WorldToGrid(worldPos);

            if (!GridUtility.IsPositionOnGrid(gridPos, _width, _height))
            {
                return;
            }
            var targetGridItem = _spawnedItems.Find(x => x.GridPos == gridPos);


            if (targetGridItem != null)
            {
                if (targetGridItem.ID != _selectedButton.ItemID)
                {
                    Destroy(targetGridItem.gameObject);
                    _spawnedItems.Remove(targetGridItem);
                }
                else
                {
                    return;
                }
            }
            

            var item = Instantiate(_gridItem);
            var itemData = _itemContainer.GetItemDataById(_selectedButton.ItemID);
            item.SpriteRenderer.sprite = itemData.Icon;
            item.ID = itemData.ID;
            item.GridPos = gridPos;
            item.ItemType = itemData.ItemType;
            var position = _gameBoard.GridToWorldCenter(gridPos);
            item.transform.position = position;
            
            _spawnedItems.Add(item);
        }

        private void CreateEditorButtons()
        {
            foreach (var item in _itemContainer.Items)
            {
                var button = Instantiate(_buttonPrefab, _selectItemParent);
                button.Construct(item.Icon, item.ID);
                _itemButtons.Add(button);
            }
        }
        private void AddOptionsToGoalDropdown()
        {
            var options = new List<TMP_Dropdown.OptionData>();
            foreach (var item in _itemContainer.Items)
            {
                var option = new TMP_Dropdown.OptionData(item.name, item.Icon);
                options.Add(option);
            }
            _levelGoalsDropdown.AddOptions(options);
        }
        private void OnAddFallItemsClicked()
        {
            if (_selectedButton == null)
                return;
            var item = Instantiate(_buttonPrefab, _addedFallItemsContainer);
            var itemData = _itemContainer.GetItemDataById(_selectedButton.ItemID);
            item.Construct(itemData.Icon, itemData.ID);
            _spawnedFallItems.Add(item);
            _fallItems.Add(item.ItemID);
        }
        private void OnClearFallItemsClicked()
        {
            _fallItems.Clear();

            foreach (var item in _spawnedFallItems)
            {
                Destroy(item.gameObject);
            }

            _spawnedFallItems.Clear();
        }
        private void OnClearGoalsClicked()
        {
            _addedGoals.Clear();
        }
        private void OnLevelDataCreateClicked()
        {
            var path = _dataPath + $"{_levelName.text}.asset";
            LevelData levelData;
            if (_editLevelData)
            {
                levelData = _editLevelData;
            }
            else
            {
                levelData = ScriptableObject.CreateInstance<LevelData>();
                AssetDatabase.CreateAsset(levelData, path);
                _editLevelData = levelData;
            }
            levelData.LevelGoals = _addedGoals;
            levelData.GridWidth = _width;
            levelData.GridHeigth = _height;
            levelData.LevelGridData = new();
            levelData.FallItems = _fallItems;

            for (int i = 0; i < _spawnedItems.Count; i++)
            {
                LevelEditorGridItem item = _spawnedItems[i];

                var levelGridData = new LevelGridData
                {
                    GridPosition = item.GridPos,
                    ID = _selectedButton.ItemID,
                    ItemType = item.ItemType,
                };

                levelData.LevelGridData.Add(levelGridData);
            }

            EditorUtility.SetDirty(levelData);
        }
        private void OnItemButtonClicked(LevelEditorItemButton button)
        {
            if(_selectedButton != null && _selectedButton.ItemID != button.ItemID)
            {
                _selectedButton.DeSelected();
            }
            _selectedButton = button;
            _selectedButton.Selected();
        }
        private void OnCreateGridClicked()
        {
            _width = int.Parse(_widthInput.text);
            _height = int.Parse(_heightInput.text);

            _gameBoard.Construct(_width, _height);
        }
        private void OnAddGoalClicked()
        {
            var itemName = _levelGoalsDropdown.options[_levelGoalsDropdown.value].text;
            var targetItem = _itemContainer.Items.Find(x => x.name.Equals(itemName));
            var goalCount = int.Parse(_levelGoal.text);
            var goal = new LevelGoal 
            { 
                ID =  targetItem.ID,
                Goal = goalCount
            };

            _addedGoals.Add(goal);
        }
    }
#endif
}

