using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonHandler : MonoBehaviour
{

    public GameObject menu;
    // Start is called before the first frame update
    public void closeMenu()
    {
        menu.GetComponent<Canvas>().enabled = false;
    }

}