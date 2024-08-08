using DG.Tweening;
using GridSystem;
using Power;
using System;
using UnityEngine;
namespace Item
{
    public interface IItem
    {
        public int ItemID { get; }
        public ItemType Type { get; }
        public PowerType PowerType { get; }
        Transform Transform { get; }
        public bool IsMatchItem { get; }
        public bool IsPoppable { get; }
        public bool IsPower { get; }
        public bool IsMoving { get; set; }
        void SetItemInfo(ItemData data);
        void Interact();
        void Pop();
        void Show();
        void Hide();
        void SetPosition(Vector3 position);
        void SetScale(float scale);
        void SetOrder(int order);
        Tween FallTween(Vector3 target, float distance, Action onComplete = null);
        Tween UIMove(Action onComplete = null);
    }
}