using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public sealed class AudioData : ScriptableObject
{
    [SerializeField] private string _audioLocation;
    [SerializeField] private string _nameRecorder;
    [SerializeField] private Vector3 _location;
    [SerializeField] private AudioType _audioType;

    public AudioClip Clip { get; private set; }
    public string AudioLocation => _audioLocation;
    public string NameRecorder => _nameRecorder;
    public Vector3 Location => _location;
    public AudioType AudioType => _audioType;

    public void Instantiate(string json) => JsonUtility.FromJsonOverwrite(json, this);


    public IEnumerator LoadAudio()
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
}
