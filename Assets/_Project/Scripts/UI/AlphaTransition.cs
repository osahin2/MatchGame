using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;
namespace UI
{
    public class AlphaTransition : MonoBehaviour
    {
        [SerializeField] private Image _alphaImage;
        [SerializeField] private float _transitionTime;
        public void Transition(Action onFaded = null, float delay = 0f, Action onComplete = null)
        {
            _alphaImage.DOFade(1f, _transitionTime)
                .SetDelay(delay)
                .OnComplete(() =>
                {
                    onFaded?.Invoke();
                    _alphaImage.DOFade(0f, _transitionTime)
                    .SetDelay(.2f)
                    .OnComplete(() =>
                    {
                        onComplete?.Invoke();
                    });
                });
        }
    }
}

