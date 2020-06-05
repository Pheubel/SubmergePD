using UnityEngine;
using UnityEngine.Video;

public class OnboardingController : MonoBehaviour
{
    [SerializeField] OnboardingStep _progres;

    [SerializeField] private VideoPlayer[] _videoPlayers;

    [Header("Videos")]
    [SerializeField] VideoClip _teleportClip;
    [SerializeField] VideoClip _sphereSelect;

    public void Start()
    {
        ChangeClip(_teleportClip);
    }

    public void HandleTeleported()
    {
        if (_progres != OnboardingStep.Start)
            return;

        _progres = OnboardingStep.HasTeleported;
        ChangeClip(_sphereSelect);
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
        Start = 0,
        HasTeleported = 1
    }
}
