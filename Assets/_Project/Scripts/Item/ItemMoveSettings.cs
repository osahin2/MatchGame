using DG.Tweening;
using UnityEngine;
namespace Item
{
    [CreateAssetMenu(fileName = "Item Move Settings", menuName ="Item/Item Move Settings")]
    public class ItemMoveSettings : ScriptableObject
    {
        [SerializeField] private float _fallSpeed;
        [SerializeField] private AnimationCurve _fallEase;
        [SerializeField] private float _uiMoveTime;
        [SerializeField] private Ease _uiMoveEase;
        [SerializeField] private float _rocketMoveTime;
        [SerializeField] private float _rocketDistance;
        [SerializeField] private Vector3 _shakeStrength;
        public float FallSpeed => _fallSpeed;
        public float UMoveTime => _uiMoveTime;
        public AnimationCurve FallEase => _fallEase;
        public Ease UIMoveEase => _uiMoveEase;
        public float RocketMoveTime => _rocketMoveTime; 
        public float RocketDistance => _rocketDistance;
        public Vector3 ShakeStrength => _shakeStrength;
    }
}