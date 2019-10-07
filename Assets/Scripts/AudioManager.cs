using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Our main theme
    [SerializeField] AudioClip _themeIntro = null;
    [SerializeField] AudioClip _themeEnding = null;
    [SerializeField] AudioClip _themeLoop = null;

    // Currently playing AudioSources and when to kill them
    private Dictionary<AudioSource, float> _audioSources = null;

    // Delayes audio sources
    private Dictionary<AudioSource, float> _delayedAudioSources = null;

    private void Awake()
    {
        _audioSources = new Dictionary<AudioSource, float>();
        _delayedAudioSources = new Dictionary<AudioSource, float>();
        Play(_themeIntro);
        Play(_themeLoop, true, _themeIntro.length);
    }

    private void Update()
    {
        AudioSource[] sources = _audioSources.Keys.ToArray();
        foreach (AudioSource source in sources)
        {
            if (Time.time >= _audioSources[source])
            {
                Stop(source);
            }
        }
    }

    private void Play(AudioClip clip, bool loop = false, float delay = 0.0f)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.loop = loop;
        source.clip = clip;
        source.PlayDelayed(delay);
        if (!loop)
        {
            _audioSources.Add(source, Time.time + clip.length + delay);
        }
    }

    private void Stop(AudioSource audioSource)
    {
        _audioSources.Remove(audioSource);
        Destroy(audioSource);
    }
}
