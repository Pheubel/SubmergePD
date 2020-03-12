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
        if (e.target.name == "Cube")
        {
            Debug.Log("Cube was clicked");
        }
        else if (e.target.name == "Button")
        {
            Debug.Log("Button was clicked");
        }

    }

    public void PointerInside(object sender, PointerEventArgs e)
    {

        if (e.target.name == "Cube")
        {
            Debug.Log("Cube was entered");
        }
        else if (e.target.name == "Button")
        {
            Debug.Log("Button was entered");
        }
        if (e.target == null)
            return;

        MeshRenderer meshRenderer = e.target.GetComponent<MeshRenderer>();
        meshRenderer.material.color = enterColor;
    }

    public void PointerOutside(object sender, PointerEventArgs e)
    {
        if (e.target.name == "Cube")
        {
            Debug.Log("Cube was exited");
        }
        else if (e.target.name == "Button")
        {
            Debug.Log("Button was exited");
        }

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