using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR.InteractionSystem;

public class TransitionConroller : MonoBehaviour
{
    public string NextScene => _nextScene;
    [SerializeField] private string _nextScene;

    [SerializeField] Player _player;

    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();

        _animator.SetTrigger("FadeInCalled");
    }

    public void OnFadeInCompleted()
    {

    }

    public void OnFadeOutCompleted()
    {
        if (_player != null)
            Destroy(_player.gameObject);

        SceneManager.LoadScene(_nextScene);
    }

    public void PlayFadeOut()
    {
        _animator.SetTrigger("FadeOutCalled");
    }
}
