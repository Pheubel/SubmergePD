using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

[Serializable]
public sealed class AudioData : ScriptableObject
{
    [SerializeField] private string _audioLocation;
    [SerializeField] private string _nameRecorder;
    [SerializeField] private Vector3 _location;
    [SerializeField] private AudioType _audioType;

    private LoadedAudioEvent _loadedClip;

    public AudioClip Clip { get; private set; }
    public string AudioLocation => _audioLocation;
    public string NameRecorder => _nameRecorder;
    public Vector3 Location { get => _location; set => _location = value; }
    public AudioType AudioType => _audioType;

    public void Instantiate(string json) => JsonUtility.FromJsonOverwrite(json, this);

    public void Instantiate(string audioLocation, Vector3 location, AudioType type, AudioClip clip)
    {
        Clip = clip;
        Instantiate(audioLocation, location, type);
    }

    public void Instantiate(string audioLocation, Vector3 location, AudioType type)
    {
        _audioLocation = audioLocation;
        _location = location;
        _audioType = type;

        _nameRecorder = "Joe";
    }

    public void SubscribeToLoadEvent(UnityAction<AudioClip> eventhandler)
    {
        if (_loadedClip == null)
            _loadedClip = new LoadedAudioEvent();

        _loadedClip.AddListener(eventhandler);
    }
        

    public IEnumerator LoadAudio()
    {
        if (Clip == null)
        {
            using (var request = UnityWebRequestMultimedia.GetAudioClip(_audioLocation, _audioType))
            {
                yield return request.SendWebRequest();

                if (request.isNetworkError || request.isHttpError)
                {
                    Debug.LogError("Error: " + request.error);
                }

                Clip = DownloadHandlerAudioClip.GetContent(request);
            }
        }

        Clip.name = _audioLocation;

        _loadedClip?.Invoke(Clip);
        _loadedClip?.RemoveAllListeners();
    }

    private class LoadedAudioEvent : UnityEvent<AudioClip> { }
}
