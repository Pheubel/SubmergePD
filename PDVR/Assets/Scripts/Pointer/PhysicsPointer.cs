using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsPointer : MonoBehaviour
{
    public float defaultLength = 7.0f;

    private LineRenderer lineRenderer = null;
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        UpdateLength();
    }

    private void UpdateLength()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, CalculateEnd());

    }
    private Vector3 CalculateEnd()
    {
        RaycastHit hit = CreateForwardRayCast();
        Vector3 endPositon = DefaultEnd(defaultLength);

        if (hit.collider)
            endPositon = hit.point;

        return endPositon;
    }

    private RaycastHit CreateForwardRayCast()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        Physics.Raycast(ray, out hit, defaultLength);
        return hit;
    }

    private Vector3 DefaultEnd(float length)
    {
        return transform.position + (transform.forward * length);
    }
}
