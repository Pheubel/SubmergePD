using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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

#if RADIAL_MENU_PROTO

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

#else

    [SerializeField] private TouchEvent touchEvent;
    [SerializeField] private PressedEvent pressedEvent;
    [SerializeField] private ReleasedEvent releasedEvent;
    [SerializeField] private PositionEvent positionEvent;

    private void Awake()
    {
        touch.AddOnChangeListener(OnTouch, _hands);
        press.AddOnStateDownListener(OnPressed, _hands);
        press.AddOnStateUpListener(OnReleased, _hands);
        touchPosition.AddOnAxisListener(OnPositionChanged,_hands);
    }
    private void OnDestroy()
    {
        touch.RemoveOnChangeListener(OnTouch, _hands);
        press.RemoveOnStateDownListener(OnPressed, _hands);
        press.RemoveOnStateUpListener(OnReleased, _hands);
        touchPosition.RemoveOnAxisListener(OnPositionChanged, _hands);
    }

    private void OnPositionChanged(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta) =>
        positionEvent.Invoke(fromAction, fromSource, axis, delta);

    private void OnTouch(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState) =>
        touchEvent?.Invoke(fromAction,fromSource,newState);

    private void OnPressed(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) =>
        pressedEvent.Invoke(fromAction, fromSource);

    private void OnReleased(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) =>
        releasedEvent.Invoke(fromAction, fromSource);

#endif

    public void Vibrate()
    {
        _hapticSignal.Execute(0, _hapticDuration, _frequency, _amplitude, _hands);
    }
}

#if !RADIAL_MENU_PROTO
[System.Serializable]
public class PositionEvent : UnityEvent<SteamVR_Action_Vector2, SteamVR_Input_Sources, Vector2, Vector2> { }
[System.Serializable]
public class PressedEvent : UnityEvent<SteamVR_Action_Boolean, SteamVR_Input_Sources> { }
[System.Serializable]
public class ReleasedEvent : UnityEvent<SteamVR_Action_Boolean, SteamVR_Input_Sources> { }
[System.Serializable]
public class TouchEvent : UnityEvent<SteamVR_Action_Boolean, SteamVR_Input_Sources, bool> { }
#endif