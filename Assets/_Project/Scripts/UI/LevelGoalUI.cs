using DG.Tweening;
using Item;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace UI
{
    public class LevelGoalUI : MonoBehaviour
    {
        [SerializeField] private Image _goalImage;
        [SerializeField] private TextMeshProUGUI _goalText;
        [SerializeField] private ParticleSystem _rippleParticle;
        [SerializeField] private float _animationScale;
        [SerializeField] private float _animationDuration;

        public int ID { get; private set; }

        private Tween _hitTween;

        public void SetGoalInfo(ItemData itemData, int goal)
        {
            _goalImage.sprite = itemData.Icon;
            ID = itemData.ID;
            SetGoalText(goal);
        }
        public void SetGoalText(int goal)
        {
            _goalText.text = goal.ToString();
        }
        public void Hit()
        {
            _hitTween?.Kill(true);
            _hitTween = _goalImage.transform.DOPunchScale(Vector3.one * _animationScale, _animationDuration);
            _rippleParticle.Play();
        }
    }
}

