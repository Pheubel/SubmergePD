using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class InputManager : MonoBehaviour
{
    [Header("Actions")]
    public SteamVR_Action_Boolean touch = null;
    public SteamVR_Action_Boolean press = null;
    public SteamVR_Action_Vector2 touchPosition = null;

    [SerializeField] SteamVR_Action_Vibration _hapticSignal;
    [SerializeField] float _hapticDuration;
    [SerializeField, Range(0, 320)] float _frequency;
    [SerializeField, Range(0, 1)] float _amplitude;

    [SerializeField] private SteamVR_Input_Sources _hands;

    [Header("Scene Objects")]
    public RadialMenu radialMenu = null;

    private void Awake()
    {
        touch.AddOnChangeListener(Touch,_hands);
        press.onStateUp += PressRelease;
        touchPosition.onAxis += Position;
    }

    private void OnDestroy()
    {
        touch.onChange -= Touch;
        press.onStateUp -= PressRelease;
        touchPosition.onAxis -= Position;
    }

    private void Position(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta)
    {
        radialMenu.SetTouchPosition(axis);
    }

    private void Touch(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        radialMenu.Show(newState);
    }

    private void PressRelease(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        radialMenu.ActivateHighlightedSection();
    }

    public void Vibrate()
    {
        _hapticSignal.Execute(0, _hapticDuration, _frequency, _amplitude, _hands);
    }
}
