using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class BlockPointer : MonoBehaviour
{
    public float distance = 40;
    float timer1=0, timer2=0;
    Ray ray;
    LineRenderer lineRenderer;
    GameObject cube;
    MeshRenderer cubeRenderer;
    public Vector3 position;
    public Vector3[] points;
    Color[] colors = new Color[16];
    // Start is called before the first frame update
    void Start()
    {
        points = new Vector3[2];
        ray = new Ray(transform.position, transform.forward);
        lineRenderer = this.gameObject.GetComponent<LineRenderer>();
        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Destroy(cube.GetComponent<BoxCollider>());
        cubeRenderer = cube.GetComponent<MeshRenderer>();

        FillColorArray();
        ChangeColor(0);
    }

    // Update is called once per frame
    void Update()
    {
        ray.origin = transform.position;
        ray.direction = transform.forward;
        position = ray.GetPoint(distance);

        points[0] = transform.position;
        for(int i=0;i<3;i++)
        {
            points[1][i] = Mathf.FloorToInt(position[i]);
        }
        cube.transform.position = points[1];
        lineRenderer.SetPositions(points);

        if (SteamVR_Actions._default.ExtendPointer.GetState(SteamVR_Input_Sources.RightHand) && timer1 <= 0.0f)
        { ChangeDistance(1); timer1 = 0.2f; }
        if (SteamVR_Actions._default.ShortenPointer.GetState(SteamVR_Input_Sources.RightHand) && timer2 <= 0.0f)
        { ChangeDistance(-1); timer2 = 0.2f; }

        if (timer1 > 0) timer1 -= Time.deltaTime;
        if (timer2 > 0) timer2 -= Time.deltaTime;

        if (SteamVR_Actions._default.ExtendPointer.GetStateUp(SteamVR_Input_Sources.RightHand))
            timer1 = 0;
        if (SteamVR_Actions._default.ShortenPointer.GetStateUp(SteamVR_Input_Sources.RightHand))
            timer2 = 0;
    }

    void ChangeDistance(int delta)
    {
        if (distance + delta > 5) distance += delta;
    }

    public void ChangeColor(int colorIndex)
    {
        lineRenderer.material.color = colors[colorIndex];
        cubeRenderer.material.color = colors[colorIndex];
    }

    void FillColorArray()
    {
        GameObject buttons = GameObject.FindWithTag("ColorUIButtons");
        for (int i = 0; i < 16; i++)
        {
            colors[i] = buttons.transform.GetChild(i).GetComponent<Button>().colors.normalColor;
        }
    }
}
