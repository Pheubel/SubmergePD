using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

public class OnboardingController : MonoBehaviour
{
    [SerializeField] GameObject _canvas;
    [SerializeField] OnboardingStep _progres;
    [SerializeField] AudioClip _stepCompleteClip;

    [SerializeField] private VideoPlayer[] _videoPlayers;
    [SerializeField] private AudioSource _audioSource;

    [Header("Videos")]
    [SerializeField] VideoClip _teleportClip;
    [SerializeField] VideoClip _sphereSelect;
    [SerializeField] VideoClip _speachRecording;
    [SerializeField] VideoClip _moveAudioBubble;

    [Header("Sin Spheres")]
    [SerializeField] GameObject _closedSphere;
    [SerializeField] GameObject _openSphere;

    [SerializeField] UnityEvent _onOnboardingComplete;

    public void Start()
    {
        var cameraTransform = Camera.main.transform;
        var newPos = (cameraTransform.forward * 190f);
        newPos.y = 0;
        _canvas.transform.position= newPos;
        _canvas.transform.LookAt(new Vector3(cameraTransform.position.x, 0, cameraTransform.position.z));
        _canvas.transform.rotation *= Quaternion.AngleAxis(180, Vector3.up);

        ChangeClip(_teleportClip);
    }

    public void HandleTeleported()
    {
        if (_progres != OnboardingStep.Teleport)
            return;

        _progres = OnboardingStep.SelectSphere;
        ChangeClip(_sphereSelect);

        _audioSource.PlayOneShot(_stepCompleteClip);

        var newPos = (Camera.main.transform.forward * 5);
        newPos.y = Camera.main.transform.position.y;
        _closedSphere.transform.position = newPos;
        _closedSphere.transform.LookAt(Camera.main.transform.position);
        _openSphere.transform.rotation = _closedSphere.transform.rotation;

        _closedSphere.SetActive(true);
    }

    public void HandleSphereSeleted()
    {
        if (_progres != OnboardingStep.SelectSphere)
            return;

        _progres = OnboardingStep.RecordAudio;
        ChangeClip(_speachRecording);

        _audioSource.PlayOneShot(_stepCompleteClip);

        Destroy(_openSphere, 5);
    }

    public void HandeAudioRecorded()
    {
        if (_progres != OnboardingStep.RecordAudio)
            return;

        _progres = OnboardingStep.Completed;
        //ChangeClip(_moveAudioBubble);


        _audioSource.Stop();
        _audioSource.PlayOneShot(_stepCompleteClip);
        Invoke(nameof(InvokeCompleted), 5f);
    }

    public void HandleAudioMoved()
    {
        if (_progres != OnboardingStep.MoveAudioBubble)
            return;

        _progres = OnboardingStep.Completed;
        ChangeClip(_moveAudioBubble);

        _audioSource.PlayOneShot(_stepCompleteClip);
    }

    private void InvokeCompleted() => _onOnboardingComplete.Invoke();

    // private void InvokeOneShot() => _audioSource.PlayOneShot(_stepCompleteClip);

    public void ChangeClip(VideoClip clip)
    {
        for (int i = 0; i < _videoPlayers.Length; i++)
        {
            _videoPlayers[i].clip = clip;
        }
    }

    public enum OnboardingStep
    {
        Teleport,
        SelectSphere,
        RecordAudio,
        MoveAudioBubble,
        Completed
    }
}
