﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSourceController),typeof(Renderer))]
public class AudioBubbleController : MonoBehaviour
{
    Vector3 _startPosition;
    Quaternion _startRotation;
    bool _isInRest;

    [Tooltip("The time in seconds before the bubble is reset to it's original position.")]
    [SerializeField] float _resetDelay;

    AudioSourceController _audioSourceController;
    Renderer _renderer;

    Coroutine _resetPositionCoroutine;

    private void Start()
    {
        _resetDelay = Mathf.Clamp(_resetDelay, 0f, float.MaxValue);

        _audioSourceController = GetComponent<AudioSourceController>();
        _renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (_isInRest)
            transform.Rotate(Vector3.up, 5f * Time.deltaTime);

        if (_audioSourceController.IsPlaying)
            _renderer.material.color = Color.red;
        else
            _renderer.material.color = Color.white;


#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.P))
            HandleOnGrab();
        if (Input.GetKeyUp(KeyCode.P))
            HandleOnRelease();
#endif
    }

    public void Initialize(Vector3 startPosition, Quaternion rotation)
    {
        _isInRest = true;

        _startRotation = rotation;
        _startPosition = startPosition;
    }

    public void HandleOnGrab()
    {
        _isInRest = false;

        transform.rotation = _startRotation;

        if (_resetPositionCoroutine != null)
        {
            StopCoroutine(_resetPositionCoroutine);
            _resetPositionCoroutine = null;
        }

        _audioSourceController.PlayAudio();
    }

    public void HandleOnRelease()
    {
        _audioSourceController.StopAudio();

        if (_resetPositionCoroutine == null)
            _resetPositionCoroutine = StartCoroutine(ResetPosition());
    }

    IEnumerator ResetPosition()
    {
        yield return new WaitForSecondsRealtime(_resetDelay);

        transform.position = _startPosition;
        _isInRest = true;
    }
}
