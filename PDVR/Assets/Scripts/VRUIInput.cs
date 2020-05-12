using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.Extras;

[RequireComponent(typeof(SteamVR_LaserPointer))]
public class VRUIInput : MonoBehaviour
{
    private SteamVR_LaserPointer _laserPointer;

    private void Start()
    {
        _laserPointer = GetComponent<SteamVR_LaserPointer>();
    }

    private void OnEnable()
    {
        _laserPointer.PointerIn += HandlePointerIn;
        _laserPointer.PointerOut += HandlePointerOut;
        _laserPointer.PointerClick += HandlePointerClick;
    }

    private void OnDisable()
    {
        _laserPointer.PointerIn -= HandlePointerIn;
        _laserPointer.PointerOut -= HandlePointerOut;
        _laserPointer.PointerClick -= HandlePointerClick;
    }

    private void HandlePointerIn(object sender, PointerEventArgs e)
    {
        // entered object
    }

    private void HandlePointerOut(object sender, PointerEventArgs e)
    {
        // exited object
    }

    private void HandlePointerClick(object sender, PointerEventArgs e)
    {
        // clicked object
        if (e.target.TryGetComponent<LaserPointerInteractable>(out var interactable))
            interactable.TriggerInteractable();
    }
}
