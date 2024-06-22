using System;
using UnityEngine;
namespace Item
{
    public interface IItem
    {
        public int ItemID { get; }
        public ItemType Type { get; }
        Transform Transform { get; }
        public bool IsMatchItem { get; }
        public bool IsPoppable { get; }
        void SetItemInfo(ItemData data);
        void Interact(Action onSuccess);
        void Show();
        void Hide();
        void SetPosition(Vector3 position);
        void SetScale(float scale);
        void SetOrder(int order);
    }
}