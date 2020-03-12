using Proyecto26;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var fileUrl = "http://localhost:3000/cue_audio";
        var fileType = AudioType.WAV;
        RestClient.Get(new RequestHelper { Uri = fileUrl, DownloadHandler = new DownloadHandlerAudioClip(fileUrl, fileType), }).Then(res => { AudioSource audio = GetComponent<AudioSource>(); audio.clip = ((DownloadHandlerAudioClip)res.Request.downloadHandler).audioClip; audio.Play(); });

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
