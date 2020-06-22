using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioBubble : MonoBehaviour
{

    [SerializeField] AudioData _recording;

    Quaternion _startRotation;
    bool _isInRest;

    public AudioSource _audioSource;
    public Slider slider;
    public TMP_Text position;


    public bool IsPlaying => _audioSource.isPlaying;
    public bool IsStopped => !_audioSource.isPlaying && _audioSource.timeSamples == 0;
    public bool IsPaused => !_audioSource.isPlaying && _audioSource.timeSamples != 0 && _audioSource.timeSamples != _audioSource.clip.samples;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        slider.maxValue = _audioSource.clip.length;
    }

    public void PlayAudio(AudioClip clip, float point = 0)
    {
        _audioSource.clip = clip;
        PlayAudio(point);
    }

    public void Initialize(AudioData data)
    {
        _isInRest = true;

        _startRotation = new Quaternion();
        _recording = data;
        _audioSource.clip = data.Clip;
    }

    public void setTime(float time)
    {
        _audioSource.time = time;
    }
    private void Update()
    {
        position.text = _audioSource.time.ToString();
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
