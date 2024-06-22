using System;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Inputs
{
    public class InputSystem : MonoBehaviour, IInputSystem, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField] private Camera _camera;

        public event EventHandler<InputEventArgs> PointerDown;
        public event EventHandler<InputEventArgs> PointerDrag;
        public event EventHandler<InputEventArgs> PointerUp;

        public void OnPointerDown(PointerEventData eventData)
        {
            PointerDown?.Invoke(this, new InputEventArgs(GetWorldPosition(eventData.position), eventData));
        }
        public void OnDrag(PointerEventData eventData)
        {
            PointerDrag?.Invoke(this, new InputEventArgs(GetWorldPosition(eventData.position), eventData));
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            PointerUp?.Invoke(this, new InputEventArgs(GetWorldPosition(eventData.position), eventData));
        }
        private Vector2 GetWorldPosition(Vector3 screenPos)
        {
            return _camera.ScreenToWorldPoint(screenPos);
        }
    }
}
