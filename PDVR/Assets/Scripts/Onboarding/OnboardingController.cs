using UnityEngine;
using UnityEngine.Video;

public class OnboardingController : MonoBehaviour
{
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

    public void Start()
    {
        ChangeClip(_teleportClip);
    }

    public void HandleTeleported()
    {
        if (_progres != OnboardingStep.Teleport)
            return;

        _progres = OnboardingStep.SelectSphere;
        ChangeClip(_sphereSelect);

        _audioSource.PlayOneShot(_stepCompleteClip);
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

        _progres = OnboardingStep.MoveAudioBubble;
        ChangeClip(_moveAudioBubble);

        _audioSource.PlayOneShot(_stepCompleteClip);
    }

    public void HandleAudioMoved()
    {
        if (_progres != OnboardingStep.MoveAudioBubble)
            return;

        _progres = OnboardingStep.Completed;
        ChangeClip(_moveAudioBubble);

        _audioSource.PlayOneShot(_stepCompleteClip);
    }

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
