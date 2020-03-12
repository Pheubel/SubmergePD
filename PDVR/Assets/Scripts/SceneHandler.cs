using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR.Extras;

public class SceneHandler : MonoBehaviour
{

    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color enterColor = Color.white;
    [SerializeField] private Color downColor = Color.white;

    public LineRenderer line;

    public SteamVR_LaserPointer laserPointer;

    void Awake()
    {
        laserPointer.PointerIn += PointerInside;
        laserPointer.PointerOut += PointerOutside;
        laserPointer.PointerSqueeze += PointerSqueeze;
        laserPointer.PointerRelease += PointerRelease;
    }

    public void PointerSqueeze(object sender, PointerEventArgs e)
    {
        if (e.target == null)
            return;

        print(e.target.position);
        line.SetPosition(0, e.target.position);

    }

    public void PointerInside(object sender, PointerEventArgs e)
    {
        if (e.target == null)
            return;

        MeshRenderer meshRenderer = e.target.GetComponent<MeshRenderer>();
        meshRenderer.material.color = enterColor;
    }

    public void PointerOutside(object sender, PointerEventArgs e)
    {
        MeshRenderer meshRenderer = e.target.GetComponent<MeshRenderer>();
        meshRenderer.material.color = normalColor;
    }

    public void PointerRelease(object sender, PointerEventArgs e)
    {
        if (e.target == null)
            return;
        print(e.target.position);
        line.SetPosition(1, e.target.position);

    }
}