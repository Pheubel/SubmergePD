using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SINSphereController : MonoBehaviour
{
    [SerializeField] GameObject _openSphere;
    [SerializeField] AudioBubbleController _audioBubble;

    public void OpenSphere()
    {
        gameObject.SetActive(false);
        _openSphere.SetActive(true);

        var audioData = ScriptableObject.CreateInstance<AudioData>();
        audioData.Instantiate($"file://{Path.Combine(Application.streamingAssetsPath, "clips", "test 1.wav")}", _audioBubble.transform.position,AudioType.WAV);
        StartCoroutine(audioData.LoadAudio());
        _audioBubble.Initialize(audioData);
    }
}
