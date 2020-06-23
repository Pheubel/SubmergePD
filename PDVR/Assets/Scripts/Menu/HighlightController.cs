using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class HighlightController : MonoBehaviour
{
    Renderer _renderer;

    [SerializeField] private Material _highlightMaterial;
    private Material _defaultMaterial;
    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _defaultMaterial = _renderer.material;
    }

    public void HandleSectionChanged(MenuSection section)
    {
        if (section != null && gameObject == section.SectionObject)
            _renderer.material = _highlightMaterial;
        else
            _renderer.material = _defaultMaterial;
    }
}
