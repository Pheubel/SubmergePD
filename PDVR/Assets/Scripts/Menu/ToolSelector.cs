using UnityEngine;

public class ToolSelector : MonoBehaviour
{
    private GameObject _activeTool;

    [SerializeField] GameObject _default;
    [SerializeField] GameObject _magnifyTool;
    [SerializeField] GameObject _sketchTool;
    [SerializeField] GameObject _meassureTool;
    [SerializeField] GameObject _recordTool;
    [SerializeField] GameObject _databaseTool;
    [SerializeField] GameObject _filterTool;

    private void Start()
    {
        _activeTool = _default;

        _default.SetActive(true);
    }

    public void ActivateDefaultTool() => ToggleTool(_default);

    public void ActivateMagnifyTool() => ToggleTool(_magnifyTool);

    public void ActivateSketchTool() => ToggleTool(_sketchTool);

    public void ActivateMeassureTool() => ToggleTool(_meassureTool);

    public void ActivateRecordTool() => ToggleTool(_recordTool);

    public void ActivateDatabaseTool() => ToggleTool(_databaseTool);

    public void ActivateFilterTool() => ToggleTool(_filterTool);

    private void ToggleTool(GameObject tool)
    {
        if (_activeTool == tool)
            return;
        if (_activeTool != null)
            _activeTool.SetActive(false);

        tool.SetActive(true);
        _activeTool = tool;
    }
}
