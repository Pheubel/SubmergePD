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
    public GameObject prefab2;
    public GameObject prefab3;
    public GameObject prefab4;
    public GameObject content;
    public GameObject menu;


    void Awake()
    {
        laserPointer.PointerIn += PointerInside;
        laserPointer.PointerOut += PointerOutside;
        laserPointer.PointerRelease += PointerRelease;
        laserPointer.PointerClick += PointerClick;

    }

    void Start()
    {
    }

    public void PointerClick(object sender, PointerEventArgs e)
    {
        if (e.target == null)
            return;

        print(e.target.position);
        line.SetPosition(0, e.target.position);
        if (e.target.name == "Cube")
        {
            Debug.Log("Cube was clicked");
            ClearChildren(e.target);
        }
        else if (e.target.name == "ButtonFotos")
        {
            Debug.Log("Fotos Button was clicked");
            ClearChildren(e.target);
            Populate(3, prefab);
        }
        else if (e.target.name == "ButtonBewijstukken")
        {
            Debug.Log("Bewijs Button was clicked");
            ClearChildren(e.target);
            Populate(8, prefab2);
        }
        else if (e.target.name == "ButtonModels")
        {
            Debug.Log("Models  Button was clicked");
            ClearChildren(e.target);
            Populate(2, prefab3);
        }
        else if (e.target.name == "ButtonFavorieten")
        {
            Debug.Log("Favorieten Button was clicked");
            ClearChildren(e.target);
            Populate(15, prefab4);
        }
        else if (e.target.name == "ButtonClose")
        {
            Debug.Log("Close Button was clicked");
            menu.GetComponent<Canvas>().enabled = false;
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

    void Populate(int numberOfItems, GameObject image)
    {
        GameObject newObj; // Create GameObject instance

        for (int i = 0; i < numberOfItems; i++)
        {
            // Create new instances of our prefab until we've created as many as we specified
            newObj = (GameObject)Instantiate(image, content.transform);

        }

    }

    void ClearChildren(Transform target)
    {
        foreach (Transform child in content.transform) {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void activateMenu()
    {
        menu.GetComponent<Canvas>().enabled = true;
    }
}