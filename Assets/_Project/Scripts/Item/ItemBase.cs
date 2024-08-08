using App;
using Audio;
using DG.Tweening;
using GridSystem;
using Inputs;
using Power;
using Service_Locator;
using System;
using System.Collections.Generic;
using UI;
using UnityEngine;
namespace Item
{
    public abstract class ItemBase : MonoBehaviour, IItem
    {
        [SerializeField] protected ItemMoveSettings _moveSettings;
        [SerializeField] protected List<ParticleSystem> Particles = new();
        [SerializeField] protected SpriteRenderer _itemIcon;
        [SerializeField] protected GameObject VisualGroup;
        [SerializeField] private ItemType _itemType;
        [SerializeField] private PowerType _powerType;

        protected ItemData Data;
        protected IGameBoard GameBoard => _gameBoard ??= ServiceProvider.Instance.Get<IGameBoard>();
        private IGameBoard _gameBoard;
        protected IGridSolver GridSolver => _gridSolver ??= ServiceProvider.Instance.Get<IGridSolver>();
        private IGridSolver _gridSolver;
        protected ILevelConditionsProvider Conditions => _conditions 
            ??= ServiceProvider.Instance.Get<ILevelConditionsProvider>();
        private ILevelConditionsProvider _conditions;
        protected IUIGoals InGameUI => _inGameUI ??= ServiceProvider.Instance.Get<IUIGoals>();
        private IUIGoals _inGameUI;
        protected IInputSystem InputSystem => _inputSystem ??= ServiceProvider.Instance.Get<IInputSystem>();
        private IInputSystem _inputSystem;

        protected IAudio Audio => _audio ??= ServiceProvider.Instance.Get<IAudio>();
        private IAudio _audio;

        public ItemType Type => _itemType;
        public PowerType PowerType => _powerType;
        public Transform Transform => transform;
        public int ItemID { get; private set; }
        public bool IsMatchItem => Data.ItemType == ItemType.MatchItem;
        public bool IsPoppable { get; private set; }
        public bool IsPower {  get; private set; }
        public bool IsMoving { get; set; }

        private Tween _shakeTween;

        public void SetItemInfo(ItemData data)
        {
            Data = data;
            if(data.Icon != null && _itemIcon != null)
            {
                _itemIcon.sprite = data.Icon;
            }
            ItemID = data.ID;
            IsPoppable = data.IsPoppable;
            IsPower = data.IsPower;
        }

        public void Show()
        {
            gameObject.SetActive(true);
            VisualGroup.SetActive(true);
        }

        public void Hide()
        {
            VisualGroup.SetActive(false);
        }
        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void SetScale(float scale)
        {
            transform.localScale = Vector3.one * scale;
        }
        public void SetOrder(int order)
        {
            if (_itemIcon == null)
                return;
            _itemIcon.sortingOrder = order;
        }
        protected void ShakeAnim()
        {
            _shakeTween?.Kill(true);
            _shakeTween = transform.DOShakeRotation(.15f, _moveSettings.ShakeStrength, randomnessMode: ShakeRandomnessMode.Harmonic);
        }
        public Tween FallTween(Vector3 target, float distance, Action onComplete = null)
        {
            return transform.DOMove(target, distance / _moveSettings.FallSpeed)
                .SetEase(_moveSettings.FallEase)
                .OnComplete(() =>
                {
                    onComplete?.Invoke();
                });
        }
        public Tween UIMove(Action onComplete = null)
        {
            var target = InGameUI.LevelGoals.Find(x => x.ID == ItemID);
            SetOrder(SortOrders.UI_MOVE);
            return transform.DOMove(target.transform.position, _moveSettings.UMoveTime)
                .SetEase(_moveSettings.UIMoveEase)
                .OnComplete(() =>
                {
                    Audio.Play(ClipType.CubeCollect);
                    onComplete?.Invoke();
                });
        }
        public abstract void Interact();
        public abstract void Pop();

    }
}