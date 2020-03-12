using UnityEngine;
using UnityEngine.Events;

public class MicrophoneRecorder : MonoBehaviour
{
    [SerializeField] RecordedEvent _clipRecorded;
    [Tooltip("The time in seconds the microphone will record for.")]
    [SerializeField] int _recordingTime = 120;
    [Tooltip("The amound of samples recorded per second.")]
    [SerializeField] int _recordFrequency = 44100;
    public string RecordingDevice
    {
        get => _recordingDevice;
        set
        {
            if (IsRecording)
            {
                Debug.LogError("Cannot assign new recording device while recording.");
            }
            else
            {
                _recordingDevice = value;
            }
        }
    }
    public bool IsRecording { get; private set; }
    public int RecordedSamples => Microphone.GetPosition(RecordingDevice);
    public float RecordingTime => RecordedSamples / (float)_recordFrequency;

    private AudioClip _clipBuffer;
    private string _recordingDevice;
    private string _recordingName;

    public void StartNewRecording(string recordingName = null)
    {
        // ends the old recording before starting a new one
        StopRecording();

        _clipBuffer = Microphone.Start(RecordingDevice, true, _recordingTime, _recordFrequency);
        IsRecording = true;
        _recordingName = recordingName;
        Invoke(nameof(StopRecording), _recordingTime);
    }

    public void StopRecording()
    {
        CancelInvoke(nameof(StopRecording));
        if (IsRecording)
        {
            int length = Microphone.GetPosition(RecordingDevice);
            Microphone.End(RecordingDevice);

            var recording = AudioClip.Create(_recordingName ?? "Microphone clip", length, 1, _recordFrequency, false);
            var samples = new float[length];

            _clipBuffer.GetData(samples, 0);
            recording.SetData(samples, 0);

            _clipRecorded?.Invoke(recording);

            IsRecording = false;
        }
    }

    [System.Serializable]
    private class RecordedEvent : UnityEvent<AudioClip> { }
}
