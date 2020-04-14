using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolControler : MonoBehaviour
{
    [SerializeField] GameObject _defaultControler;
    [SerializeField] GameObject _magnifyTool;
    [SerializeField] GameObject _recordTool;

    [SerializeField] GameObject _toolSelectionInterface;

    bool _interfaceActive = false;

    GameObject _activeTool;

    private void Start()
    {
        _magnifyTool.SetActive(false);
        _recordTool.SetActive(false);
        _toolSelectionInterface.SetActive(_interfaceActive);
        _activeTool = _defaultControler;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            _interfaceActive = !_interfaceActive;
            _toolSelectionInterface.SetActive(_interfaceActive);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (_activeTool != _magnifyTool)
                _activeTool.SetActive(false);

            _magnifyTool.SetActive(true);
            _activeTool = _magnifyTool;
             _interfaceActive = false;

            _toolSelectionInterface.SetActive(_interfaceActive);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (_activeTool != _recordTool)
                _activeTool.SetActive(false);

            _recordTool.SetActive(true);
            _activeTool = _recordTool;
            _interfaceActive = false;

            _toolSelectionInterface.SetActive(_interfaceActive);
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            if (_activeTool != _defaultControler)
                _activeTool.SetActive(false);

            _defaultControler.SetActive(true);
            _activeTool = _defaultControler;
            _interfaceActive = false;

            _toolSelectionInterface.SetActive(_interfaceActive);
        }
    }
}
