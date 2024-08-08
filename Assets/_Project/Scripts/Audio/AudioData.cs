using UnityEngine;

namespace Audio
{
    [CreateAssetMenu(fileName = "Audio Data", menuName ="Audio/Audio Data")]
    public class AudioData : ScriptableObject
    {
        [SerializeField] private ClipType _type;
        [SerializeField] private AudioClip _clip;
        public ClipType Type => _type;
        public AudioClip Clip => _clip;
    }
}

