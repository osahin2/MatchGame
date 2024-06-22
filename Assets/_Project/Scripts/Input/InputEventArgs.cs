using System;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Inputs
{
    public class InputEventArgs : EventArgs
    {
        public Vector2 WorldPosition { get; }
        public PointerEventData PointerEventData { get; }

        public InputEventArgs(Vector2 worldPosition, PointerEventData eventData)
        {
            WorldPosition = worldPosition;
            PointerEventData = eventData;
        }
    }
}
