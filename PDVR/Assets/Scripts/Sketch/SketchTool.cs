using UnityEngine;
using Valve.VR;

public class SketchTool : MonoBehaviour
{
    [Header("Materials")]
    [SerializeField] private Material _greenMat;
    [SerializeField] private Material _redMat;
    [SerializeField] private Material _purpleMat;

    [Header("Control")]
    [SerializeField] private SteamVR_Action_Boolean _triggerAction;
    [SerializeField] private SteamVR_Input_Sources _inputSource;

    [Header("Properties")]
    [SerializeField] private GameObject _drawPoint;
    [SerializeField] private MeshRenderer _tip;
    [SerializeField] private float _lineWidth = 0.1f;

    private MeshLineRenderer _currentLine;
    private bool _isDrawing;

    private LineColor _currentColor = LineColor.Green;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
            HandleTriggerDown(default,default);

        if (!_isDrawing)
            return;

        if(_currentLine == null)
        {
            Debug.Log($"_currentLine is null.");
            return;
        }

        _currentLine.AddPoint(_drawPoint.transform.position);
    }

    public void SetGreen()
    {
        _currentColor = LineColor.Green;
        _tip.material = GetSelectedMaterial();
    }

    public void SetRed() {
        _currentColor = LineColor.Red;
        _tip.material = GetSelectedMaterial();
    }

    public void SetPurple() { 
        _currentColor = LineColor.Purple;
        _tip.material = GetSelectedMaterial();
    }

    private void HandleTriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        _isDrawing = false;
        _currentLine = null;
    }

    private void HandleTriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        _isDrawing = true;

        var curMat = GetSelectedMaterial();

        var lineObject = new GameObject("line", typeof(MeshFilter), typeof(MeshRenderer));
        _currentLine = lineObject.AddComponent<MeshLineRenderer>();
        _currentLine.setWidth(_lineWidth);
        _currentLine.lmat = curMat;
    }

    private Material GetSelectedMaterial()
    {
        switch (_currentColor)
        {
            case LineColor.Green:
                return _greenMat;
            case LineColor.Red:
                return _redMat;
            case LineColor.Purple:
                return _purpleMat;
            default:
                Debug.LogError($"Unhandled line color: {_currentColor}");
                return null;
        }
    }

    private void OnEnable()
    {
        _triggerAction.AddOnStateDownListener(HandleTriggerDown, _inputSource);
        _triggerAction.AddOnStateUpListener(HandleTriggerUp, _inputSource);

        _tip.material = GetSelectedMaterial();
    }

    private void OnDisable()
    {
        _triggerAction.RemoveOnStateDownListener(HandleTriggerDown, _inputSource);
        _triggerAction.AddOnStateUpListener(HandleTriggerUp, _inputSource);

        _currentLine = null;
        _isDrawing = false;
    }

    public enum LineColor
    {
        Green,
        Red,
        Purple
    }
}
