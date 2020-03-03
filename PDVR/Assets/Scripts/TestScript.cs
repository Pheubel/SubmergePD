using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TestScript : MonoBehaviour
{
    [SerializeReference] AudioData _data;
    [SerializeField] AudioSource _source;
    [SerializeField] private bool recording;

    Coroutine loadProcces;
    // Start is called before the first frame update
    void Start()
    {
        _source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(recording && Input.GetKeyUp(KeyCode.R))
        {
            recording = false;
        }

        if(!recording && Input.GetKeyDown(KeyCode.R))
        {
            recording = true;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            loadProcces = StartCoroutine(_data.LoadAudio());
        }

        if (Input.GetKeyDown(KeyCode.P) && _source.clip != null)
            _source.Play();
    }
}
