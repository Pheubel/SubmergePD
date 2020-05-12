using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;

public class ToolControler : MonoBehaviour
{
    [SerializeField] GameObject[] _tools;
    [SerializeField] GameObject _default;

    [SerializeField] GameObject _toolSelectionInterface;

    GameObject _activeTool;
    int _selectedIndex = -1;

    private Vector2 touchPosition = Vector2.zero;

    [SerializeField] private float degreeIncrement = 40.0f;
    [SerializeField] private float offsetDegree = 0f;

    [SerializeField] UnityEvent<int> _highlightChanged;

    private void Start()
    {
        _activeTool = _default;
    }

    public void ToggleInterface(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        _toolSelectionInterface.SetActive(newState);
    }

    public void SelectCurrentHighlightedTool()
    {
        if (!_toolSelectionInterface.activeSelf)
            return;

        _activeTool.SetActive(false);

        // determine which tool is selected
        _activeTool = _selectedIndex != -1 ?
            _tools[_selectedIndex] :
            _default;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = Vector2.zero + touchPosition;
        float rotation = (getDegree(direction) + offsetDegree) % 360f;

        SetSelectedEvent(rotation);
    }

    private float getDegree(Vector2 direction)
    {
        float value = Mathf.Atan2(direction.x, direction.y);
        value *= Mathf.Rad2Deg;

        if (value < 0)
            value += 360.0f;

        return value;
    }

    private int GetNearestIncrement(float rotation)
    {
        return Mathf.RoundToInt(rotation / degreeIncrement);
    }

    private void SetSelectedEvent(float currentRotation)
    {
        int index = GetNearestIncrement(currentRotation);

        if (index > _tools.Length - 1 || index < 0)
            _selectedIndex = -1;
        else
            _selectedIndex = index;

        _highlightChanged.Invoke(_selectedIndex);
    }

    public void SetTouchPosition(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta)
    {
        touchPosition = axis;
    }
}
