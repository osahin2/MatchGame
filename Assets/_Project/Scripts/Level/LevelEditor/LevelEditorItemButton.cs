using System;
using UnityEngine;
using UnityEngine.UI;

namespace Level
{
#if UNITY_EDITOR
    public class LevelEditorItemButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private float _selectedScale = 1.3f;

        public int ItemID {  get; private set; }

        public void Construct(Sprite icon, int id)
        {
            _button.image.sprite = icon;
            ItemID = id;
        }
        public void Selected()
        {
            transform.localScale = Vector3.one * _selectedScale;
        }
        public void DeSelected()
        {
            transform.localScale = Vector3.one;
        }
        public void AddListener(Action<LevelEditorItemButton> onClicked)
        {
            _button.onClick.AddListener(() =>
            {
                onClicked?.Invoke(this);
            });
        }
        public void RemoveAllListener()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
#endif
}

