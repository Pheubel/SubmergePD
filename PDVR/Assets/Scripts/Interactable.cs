using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Interactable : MonoBehaviour
{
    [HideInInspector]
    public HandInteraction m_ActiveHand = null;
    public bool isReverse = true;
    public bool isAngled = false;

    void Update()
    {
        if (Camera.main)
        {
            var cameraTransform = Camera.main.gameObject.transform;
            transform.LookAt(cameraTransform);
            if (isReverse) transform.forward *= -1;
            if (isAngled) transform.Rotate(0, 90, 0);
        }
    }
    }
