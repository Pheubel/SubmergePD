using UnityEngine;
using Valve.VR;

public class AudioToolController : MonoBehaviour
{
    [SerializeField] AudioBubbleController _bubblePrefab;
    [Header("Input")]
    [SerializeField] SteamVR_Action_Boolean _recordAction;
    [SerializeField] SteamVR_Input_Sources _inputSource;

    MicrophoneRecorder _recorder;

    // Start is called before the first frame update
    void Start()
    {
        _recorder = GetComponent<MicrophoneRecorder>();

        _recordAction.AddOnStateUpListener(HandleRecordActionUp, _inputSource);
        _recordAction.AddOnStateDownListener(HandleRecordActionDown, _inputSource);
    }

    private void HandleRecordActionUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        _recorder.StopRecording();
    }

    private void HandleRecordActionDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        _recorder.StartNewRecording();
    }

    public void HandleClipRecorded(AudioClip clip)
    {
        var newBubble = Instantiate(_bubblePrefab);
        var data = ScriptableObject.CreateInstance<AudioData>();

        var fileLocation = "file:///" + Utilities.SavWav.Save($"{clip.name}.wav", clip, true);

        data.Instantiate(fileLocation, Vector3.zero, AudioType.WAV, clip);
        newBubble.Initialize(data);
    }

    private void OnDestroy()
    {
        _recordAction.RemoveOnStateUpListener(HandleRecordActionUp, _inputSource);
        _recordAction.RemoveOnStateDownListener(HandleRecordActionDown, _inputSource);
    }

#if UNITY_EDITOR // only have the update loop while in the editor for debugging

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            HandleRecordActionDown(_recordAction, _inputSource);
        else if(Input.GetKeyUp(KeyCode.R))
            HandleRecordActionUp(_recordAction, _inputSource);
    }

#endif
}
