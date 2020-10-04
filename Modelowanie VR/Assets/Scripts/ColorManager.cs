using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ColorManager : MonoBehaviour
{
    Transform playerTransform;
    Transform cameraTransform;
    RectTransform colorSelectRectTransform;
    Canvas colorSelectCanvas;
    LineRenderer uiPointer;

    public Vector3 target;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        cameraTransform = playerTransform.transform.GetChild(0).Find("Camera").transform;
        colorSelectRectTransform = GameObject.FindWithTag("ColorUI").GetComponent<RectTransform>();
        colorSelectCanvas = GameObject.FindWithTag("ColorUI").GetComponent<Canvas>();
        uiPointer = GameObject.FindWithTag("UIPointer").GetComponent<LineRenderer>();
        uiPointer.enabled = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (SteamVR_Actions._default.ChangeColor.GetStateDown(SteamVR_Input_Sources.Any))
        {
            ShowHideCanvas();
        }
        //UI ma byc nieco oddalone od kamery
        target = playerTransform.position + cameraTransform.forward*2;
        if (colorSelectCanvas.enabled)
        {
            uiPointer.enabled = true;
            if (Vector3.Distance(colorSelectRectTransform.position, target) > 2.5 || Vector3.Distance(colorSelectRectTransform.position, target) < 0.5)
            {
                //colorSelectRectTransform.Translate(target - colorSelectRectTransform.position * Time.deltaTime);
                colorSelectRectTransform.anchoredPosition3D = Vector3.Lerp(colorSelectRectTransform.position, target, 5 * Time.deltaTime);
            }
            colorSelectRectTransform.rotation = Quaternion.LookRotation(colorSelectRectTransform.position - playerTransform.position);
        }
    }

    public void ShowHideCanvas()
    {
        if (!colorSelectCanvas.enabled)
        {
            colorSelectCanvas.enabled = true;
            colorSelectRectTransform.position = target;
        }
        else
        {
            colorSelectCanvas.enabled = false;
            uiPointer.enabled = false;
        }
    }
}
