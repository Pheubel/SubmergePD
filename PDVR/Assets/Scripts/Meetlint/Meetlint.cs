using System;
using UnityEngine;
using Valve.VR;

public class Meetlint : MonoBehaviour
{
    [Tooltip("The amount of Unity Units that equate to 1 real world meter.")]
    [SerializeField] private float _scale = 1;

    [SerializeField] private SteamVR_Action_Boolean _triggerAction;
    [SerializeField] private SteamVR_Input_Sources _inputSource;
    [SerializeField] private GameObject _measurePoint;
    [SerializeField] private Line _lineTemplate;

    MeasureMode ActiveMeasureMode { get; set; }

    private bool _activated;
    private Line _activeLine;
    private TextMesh _textMesh;

    void Update()
    {
        if (_activated)
        {
            _activeLine.SetEndPositionDirty(_measurePoint.transform.position);
            SetLengthText(_activeLine.RawLengthDirty);
        }
    }

    public void HandleTrigger(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (_activated)
        {
            _activeLine.SetEndPosition(_measurePoint.transform.position);
            SetLengthText(_activeLine.RawLength);

            _activated = false;
        }
        else
        {
            _activeLine = Instantiate(_lineTemplate);
            if (!_activeLine.Initialized)
                _activeLine.Initialize();
            _activeLine.SetPositionsDirty(_measurePoint.transform.position, _measurePoint.transform.position);

            _activated = true;
        }
    }

    public void SetActiveLine(Line line)
    {
        _activeLine = line;
    }

    private void SetLengthText(float length)
    {
        switch (ActiveMeasureMode)
        {
            case MeasureMode.Meter:
                _textMesh.text = string.Format("Length: {0:0.###} meters", length * _scale);
                break;
            case MeasureMode.Centimeter:
                _textMesh.text = string.Format("Length: {0:0.#} centimeters", (length * _scale) * 100);
                break;
            case MeasureMode.Millimeter:
                _textMesh.text = string.Format("Length: {0:0.#} millimeters", (length * _scale) * 1000);
                break;
            default: throw new System.NotImplementedException();
        };
    }

    public void UseMillimeters()
    {
        ActiveMeasureMode = MeasureMode.Millimeter;

        if (_activeLine != null)
            SetLengthText(_activeLine.RawLengthDirty);
    }

    public void UseCentimeters()
    {
        ActiveMeasureMode = MeasureMode.Centimeter;

        if (_activeLine != null)
            SetLengthText(_activeLine.RawLengthDirty);
    }

    public void UseMeters()
    {
        ActiveMeasureMode = MeasureMode.Meter;

        if (_activeLine != null)
            SetLengthText(_activeLine.RawLengthDirty);
    }

    private void Start()
    {
        _textMesh = GetComponentInChildren<TextMesh>();
    }

    private void OnEnable()
    {
        _triggerAction.AddOnStateDownListener(HandleTrigger, _inputSource);
    }

    private void OnDisable()
    {
        _triggerAction.RemoveOnStateDownListener(HandleTrigger, _inputSource);

        if (_activated && _activeLine.enabled)
            Destroy(_activeLine);
        _activeLine = null;
    }

    public enum MeasureMode
    {
        Meter,
        Centimeter,
        Millimeter
    }
}
