using System;
using UnityEngine;
namespace Item
{
    public abstract class ItemBase : MonoBehaviour, IItem
    {
        [SerializeField] private SpriteRenderer _itemIcon;
        [SerializeField] private ItemType _itemType;

        private ItemData _data;

        public ItemType Type => _itemType;
        public Transform Transform => transform;
        public int ItemID { get; private set; }
        public bool IsMatchItem => _data.ItemType == ItemType.MatchItem;
        public bool IsPoppable { get; private set; }
        protected bool IsInteractable => IsMatchItem;

        public void SetItemInfo(ItemData data)
        {
            _data = data;
            _itemIcon.sprite = data.Icon;
            ItemID = data.ID;
            IsPoppable = data.IsPoppable;
        }
        public abstract void Interact(Action onSuccess);

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
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
            _itemIcon.sortingOrder = order;
        }
    }
}