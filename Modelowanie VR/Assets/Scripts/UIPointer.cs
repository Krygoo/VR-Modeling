using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPointer : MonoBehaviour
{
    public float length = 5;
    LineRenderer lineRenderer;
    public VRInputModule inputModule;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit = CreateRaycast(length);
        lineRenderer.SetPosition(0, transform.position);
        if (hit.collider == null)
            lineRenderer.SetPosition(1, transform.position + (transform.forward * length));
        else
            lineRenderer.SetPosition(1, hit.point);
    }

    private RaycastHit CreateRaycast(float distance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out RaycastHit hit, length);
        return hit;
    }
}
