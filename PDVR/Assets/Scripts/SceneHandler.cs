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
    public GameObject prefab; // This is our prefab object that will be exposed in the inspector

    public int numberToCreate; // number of objects to create. Exposed in inspector


    void Awake()
    {
        laserPointer.PointerIn += PointerInside;
        laserPointer.PointerOut += PointerOutside;
        laserPointer.PointerSqueeze += PointerSqueeze;
        laserPointer.PointerRelease += PointerRelease;
    }

    void Start()
    {
        Populate(3);
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
        else if (e.target.name == "ButtonFotos")
        {
            Debug.Log("Fotos Button was clicked");
            Populate(3);
        }
        else if (e.target.name == "ButtonBewijstukken")
        {
            Debug.Log("Bewijs Button was clicked");
            Populate(8);
        }
        else if (e.target.name == "ButtonModels")
        {
            Debug.Log("Models  Button was clicked");
            Populate(2);
        }
        else if (e.target.name == "ButtonFavorieten")
        {
            Debug.Log("Favorieten Button was clicked");
            Populate(15);
        }

    }

    public void PointerInside(object sender, PointerEventArgs e)
    {

        Debug.Log(e.target.name);
        if (e.target.name == "Cube")
        {
            Debug.Log("Cube was entered");
        }
        else if (e.target.name =="Button")
        {
            Image buttonImage = e.target.GetComponent<Image>();
            buttonImage.color = Color.red;
        }
        if (e.target == null)
            return;
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
            Image buttonImage = e.target.GetComponent<Image>();
            buttonImage.color = Color.white;
        }

    }

    public void PointerRelease(object sender, PointerEventArgs e)
    {
        if (e.target == null)
            return;
        print(e.target.position);
        line.SetPosition(1, e.target.position);

    }

    void Populate(int numberOfItems)
    {
        ClearChildren();
        GameObject newObj; // Create GameObject instance

        for (int i = 0; i < numberOfItems; i++)
        {
            // Create new instances of our prefab until we've created as many as we specified
            newObj = (GameObject)Instantiate(prefab, transform);

        }

    }

    void ClearChildren()
    {
        int i = 0;
        GameObject[] allChildren = new GameObject[prefab.transform.childCount];
        foreach (GameObject child in prefab.transform)
        {
            allChildren[i] = child.gameObject;
            i += 1;
        }

        foreach (GameObject child in allChildren)
        {
            DestroyImmediate(child.gameObject);
        }
      
    }
}