using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    public class AudioManager : MonoBehaviour, IAudio
    {
        [SerializeField] private List<AudioSource> _audioSources = new();
        [SerializeField] private List<AudioData> _audioDataList = new();

        public void Play(ClipType type)
        {
            var audio = _audioDataList.Find(x => x.Type == type);
            if(audio == null)
            {
                return;
            }
            var source = _audioSources.Find(x => !x.isPlaying);
            if(source == null)
            {
                var newSource = gameObject.AddComponent<AudioSource>();
                newSource.clip = audio.Clip;
                _audioSources.Add(newSource);
                return;
            }
            source.clip = audio.Clip;
            source.Play();
        }
    }
}

