using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class TestScript : MonoBehaviour
{
    [SerializeReference] AudioData _data;
    [SerializeField] AudioSource _source;
    [SerializeField] UnityAction<AudioClip> _clipRecorded;

    AudioClip _recording;

    Coroutine loadProcces;
    bool _isRecording;

    string[] _inputDevices;
    [SerializeField] string _inputDevice;
    int _inputIndex;

    [SerializeField] string _fileLocation;

    [SerializeField] AudioClip _fileClip;

    // Start is called before the first frame update
    void Start()
    {
        _source = GetComponent<AudioSource>();
        _inputDevices = Microphone.devices;
        _inputDevice = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _inputIndex = (--_inputIndex) < 0 ? _inputIndex + _inputDevices.Length : _inputIndex;
            _inputDevice = _inputDevices[_inputIndex];
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _inputIndex = (_inputIndex + 1) % _inputDevices.Length;
            _inputDevice = _inputDevices[_inputIndex];
        }

        if (Input.GetKeyDown(KeyCode.B))
            ;

        if (Input.GetKeyUp(KeyCode.R))
        {
            if (_isRecording)
            {
                int length = Microphone.GetPosition(_inputDevice);
                Microphone.End(_inputDevice);

                if (_recording != null)
                {
                    var recording = AudioClip.Create(_recording.name, length, 1, 44100, false);
                    var samples = new float[length];

                    _recording.GetData(samples, 0);
                    recording.SetData(samples, 0);

                    _clipRecorded?.Invoke(_recording);

                    var fileLocation = "file:///" + Utilities.SavWav.Save("testRec.wav", recording, true);

                    _data = ScriptableObject.CreateInstance<AudioData>();
                    _data.Instantiate(fileLocation, Vector3.zero, AudioType.WAV, recording);

                    _recording = null;
                }

                _isRecording = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && !_isRecording)
        {
            _recording = Microphone.Start(_inputDevice, true, 120, 44100);

            _isRecording = true;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            _data = ScriptableObject.CreateInstance<AudioData>();
            _data.Instantiate(_fileLocation, Vector3.zero, AudioType.WAV);
        }

        if (Input.GetKeyDown(KeyCode.L) && _data != null)
        {
            if (_data.Clip == null)
            {
                _data.SubscribeToLoadEvent((clip) =>
                {
                    _fileClip = clip;
                });
                loadProcces = StartCoroutine(_data.LoadAudio());
            }
            else
            {
                _fileClip = _data.Clip;
            }
        }
    }
}
