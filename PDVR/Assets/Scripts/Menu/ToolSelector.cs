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
        if (tool.CompareTag("screen"))
        {
            if (_filterTool.gameObject.activeSelf)
                _filterTool.gameObject.SetActive(false);

            var cameraTransform = Camera.main.gameObject.transform;
            var toolTransform = tool.transform;
            toolTransform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + 0.3f, Camera.main.transform.position.z) + Camera.main.transform.forward * 1f;
            toolTransform.rotation = new Quaternion(0.0f, Camera.main.transform.rotation.y, 0.0f, Camera.main.transform.rotation.w);
            toolTransform.LookAt(cameraTransform);
            toolTransform.rotation.Set(toolTransform.rotation.x, -toolTransform.rotation.y, toolTransform.rotation.z, toolTransform.rotation.w);
            tool.SetActive(true);

        }

        else if (tool.CompareTag("filterscreen"))
        {
            if (_databaseTool.gameObject.activeSelf)
                _databaseTool.gameObject.SetActive(false);

            var cameraTransform = Camera.main.gameObject.transform;
            tool.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z) + Camera.main.transform.forward * 1f;
            tool.transform.rotation = new Quaternion(0.0f, Camera.main.transform.rotation.y, 0.0f, Camera.main.transform.rotation.w);
            tool.transform.LookAt(cameraTransform);
            tool.SetActive(true);
        }

        else
        {
            if (_activeTool == tool)
                return;

            if (_filterTool.gameObject.activeSelf)
                _filterTool.gameObject.SetActive(false);
            if (_databaseTool.gameObject.activeSelf)
                _databaseTool.gameObject.SetActive(false);

            if (_activeTool != null)
                _activeTool.SetActive(false);

            tool.SetActive(true);
            _activeTool = tool;
        }

    }
}
