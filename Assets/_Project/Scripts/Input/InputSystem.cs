using System;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Inputs
{
    public class InputSystem : MonoBehaviour, IInputSystem, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        public event EventHandler<InputEventArgs> PointerDown;
        public event EventHandler<InputEventArgs> PointerDrag;
        public event EventHandler<InputEventArgs> PointerUp;

        [SerializeField] private Camera _camera;

        private bool _isInputActive = true;
        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_isInputActive)
            {
                return;
            }
            PointerDown?.Invoke(this, new InputEventArgs(GetWorldPosition(eventData.position), eventData));
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (!_isInputActive)
            {
                return;
            }
            PointerDrag?.Invoke(this, new InputEventArgs(GetWorldPosition(eventData.position), eventData));
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            if (!_isInputActive)
            {
                return;
            }
            PointerUp?.Invoke(this, new InputEventArgs(GetWorldPosition(eventData.position), eventData));
        }
        public void SetInputActive(bool active)
        {
            _isInputActive = active;
        }
        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
        private Vector2 GetWorldPosition(Vector3 screenPos)
        {
            return _camera.ScreenToWorldPoint(screenPos);
        }

    }
}
