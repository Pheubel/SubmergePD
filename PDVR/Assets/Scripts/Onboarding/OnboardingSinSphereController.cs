using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnboardingSinSphereController : MonoBehaviour
{
    [SerializeField] GameObject OpenSphere;

    public void HandleSphereSelected()
    {
        OpenSphere.transform.position = transform.position;
        OpenSphere.SetActive(true);
        gameObject.SetActive(false);
    }
}
