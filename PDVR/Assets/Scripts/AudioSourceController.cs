using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class AudioSourceController : MonoBehaviour
{
    private AudioSource _audioSource;

    public bool IsPlaying => _audioSource.isPlaying;
    public bool IsStopped => !_audioSource.isPlaying && _audioSource.timeSamples == 0;
    public bool IsPaused => !_audioSource.isPlaying && _audioSource.timeSamples != 0 && _audioSource.timeSamples != _audioSource.clip.samples;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudio(float point = 0f)
    {
        if (_audioSource.clip == null)
        {
            Debug.LogError($"No audio clip located in {name}'s audio source.");
            return;
        }

        if (point < 0f || point >= 1f)
        {
            Debug.LogWarning($"expected a value between 0 and 1");
            return;
        }

        if (_audioSource.isPlaying)
        {
            Debug.LogWarning($"{_audioSource.name} is already playing.");
            return;
        }

        _audioSource.timeSamples = (int)(point * _audioSource.clip.samples);
        _audioSource.Play();
    }

    public void PauseAudio() => _audioSource.Pause();
    public void ResumeAudio() => _audioSource.UnPause();

    public void StopAudio()
    {
        if (!_audioSource.isPlaying)
            return;

        _audioSource.Stop();
    }
}
