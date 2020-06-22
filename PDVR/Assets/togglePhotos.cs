using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class togglePhotos : MonoBehaviour
{
    public GameObject toggleOn;
    public GameObject toggleoff;
    private bool active = true;

    public void ToggleImages()
    {
        if (active)
        {
            active = false;
            toggleOn.SetActive(false);
            toggleoff.SetActive(true);
            object[] obj = FindObjectsOfType<GameObject>();

            foreach (GameObject o in obj)
            {
                if (o.TryGetComponent<CustomTag>(out CustomTag tag))
                {
                    if (tag.HasTag("image"))
                    {
                        o.GetComponent<MeshRenderer>().enabled = false;
                    }


                }
            }
        } else
        {
            active = true;
            toggleOn.SetActive(true);
            toggleoff.SetActive(false);

            object[] obj = FindObjectsOfType<GameObject>();

            foreach (GameObject o in obj)
            {
                if (o.TryGetComponent<CustomTag>(out CustomTag tag))
                {
                    if (tag.HasTag("image"))
                    {
                        o.GetComponent<MeshRenderer>().enabled = true;
                    }


                }
            }
        }
       
    }
}
