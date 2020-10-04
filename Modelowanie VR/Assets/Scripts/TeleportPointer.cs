using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class TeleportPointer : MonoBehaviour
{
    public float distance = 10;
    float timer1 = 0, timer2 = 0;
    public bool render = false;
    Ray ray;
    LineRenderer lineRenderer;
    GameObject capsule;
    MeshRenderer capsuleRenderer;
    public Vector3 position;
    Vector3[] points;
    // Start is called before the first frame update
    void Start()
    {
        points = new Vector3[2];
        ray = new Ray(transform.position, transform.forward);
        lineRenderer = this.gameObject.GetComponent<LineRenderer>();
        lineRenderer.material.color = new Color(1, 1, 0, 0.25f);
        capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        capsuleRenderer = capsule.GetComponent<MeshRenderer>();
        Destroy(capsule.GetComponent<BoxCollider>());
        capsuleRenderer.material = lineRenderer.material;

        if (render)
        {
            lineRenderer.enabled = true;
            capsuleRenderer.enabled = true;
        }
        else
        {
            lineRenderer.enabled = false;
            capsuleRenderer.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (render)
        {
            lineRenderer.enabled = true;
            capsuleRenderer.enabled = true;
        }
        else
        {
            lineRenderer.enabled = false;
            capsuleRenderer.enabled = false;
        }

        ray.origin = transform.position;
        ray.direction = transform.forward;
        position = ray.GetPoint(distance);

        points[0] = transform.position;
        points[1] = position;
        
        capsule.transform.position = points[1];
        lineRenderer.SetPositions(points);

        if (SteamVR_Actions._default.ExtendPointer.GetState(SteamVR_Input_Sources.LeftHand) && timer1 <= 0.0f)
        { ChangeDistance(1); timer1 = 0.2f; }
        if (SteamVR_Actions._default.ShortenPointer.GetState(SteamVR_Input_Sources.LeftHand) && timer2 <= 0.0f)
        { ChangeDistance(-1); timer2 = 0.2f; }

        if (timer1 > 0) timer1 -= Time.deltaTime;
        if (timer2 > 0) timer2 -= Time.deltaTime;

        if (SteamVR_Actions._default.ExtendPointer.GetStateUp(SteamVR_Input_Sources.LeftHand))
            timer1 = 0;
        if (SteamVR_Actions._default.ShortenPointer.GetStateUp(SteamVR_Input_Sources.LeftHand))
            timer2 = 0;
    }

    void ChangeDistance(int delta)
    {
        if (distance + delta > 3) distance += delta;
    }
}
