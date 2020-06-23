using UnityEngine;

[RequireComponent(typeof(LineRenderer), typeof(MeshCollider))]
public class Line : MonoBehaviour
{
    private bool _isDirty;
    private float _rawLength;
    private Mesh _mesh;

    private LineRenderer _lineRenderer;
    private MeshCollider _meshCollider;

    public bool Initialized { get; private set; }

    public float RawLength
    {
        get
        {
            if (_isDirty)
                Bake();

            return _rawLength;
        }
    }

    public float RawLengthDirty =>
        Vector3.Distance(_lineRenderer.GetPosition(0), _lineRenderer.GetPosition(1));

    public void SetStartPosition(Vector3 pos)
    {
        _lineRenderer.SetPosition(0, pos);
        Bake();
    }

    public void SetStartPositionDirty(Vector3 pos)
    {
        _lineRenderer.SetPosition(0, pos);
        _isDirty = true;
    }

    public void SetEndPosition(Vector3 pos)
    {
        _lineRenderer.SetPosition(1, pos);
        Bake();
    }

    public void SetEndPositionDirty(Vector3 pos)
    {
        _lineRenderer.SetPosition(1, pos);
        _isDirty = true;
    }

    public void SetPositions(Vector3 startPosition, Vector3 endPosition)
    {
        _lineRenderer.SetPositions(new Vector3[] { startPosition, endPosition });
        Bake();
    }
    public void SetPositionsDirty(Vector3 startPosition, Vector3 endPosition)
    {
        _lineRenderer.SetPositions(new Vector3[] { startPosition, endPosition });
        _isDirty = true; 
    }


    public void Bake()
    {
        _rawLength = Vector3.Distance(_lineRenderer.GetPosition(0), _lineRenderer.GetPosition(1));
        _lineRenderer.BakeMesh(_mesh, true);
        _meshCollider.sharedMesh = _mesh;

        _isDirty = false;
    }

    private void Start()
    {
        if (!Initialized)
            Initialize();
    }

    public void Initialize()
    {
        Initialized = true;

        _lineRenderer = GetComponent<LineRenderer>();
        _meshCollider = GetComponent<MeshCollider>();

        _mesh = new Mesh();
    }
}
