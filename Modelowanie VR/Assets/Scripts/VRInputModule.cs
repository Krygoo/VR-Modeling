using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;

public class VRInputModule : BaseInputModule
{
    public Camera pointerCamera;
//    public SteamVR_Input_Sources targetSource;

    private GameObject currentObject = null;
    private PointerEventData peData = null;

    protected override void Awake()
    {
        base.Awake();
        peData = new PointerEventData(eventSystem);
    }

    public override void Process()
    {
        peData.Reset();
        peData.position = new Vector2(pointerCamera.pixelWidth / 2, pointerCamera.pixelHeight / 2);

        eventSystem.RaycastAll(peData, m_RaycastResultCache);
        peData.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
        currentObject = peData.pointerCurrentRaycast.gameObject;

        m_RaycastResultCache.Clear();

        HandlePointerExitAndEnter(peData, currentObject);

        if (SteamVR_Actions._default.InteractUI.GetStateDown(SteamVR_Input_Sources.Any))
            ProcessPress(peData);

        if (SteamVR_Actions._default.InteractUI.GetStateUp(SteamVR_Input_Sources.Any))
            ProcessRelease(peData);
        //Mozna uzyc targetSource
    }

    public PointerEventData GetData()
    {
        return peData;
    }

    private void ProcessPress(PointerEventData data)
    {
        peData.pointerPressRaycast = data.pointerCurrentRaycast;

        GameObject newPointerPress = ExecuteEvents.ExecuteHierarchy(currentObject, data, ExecuteEvents.pointerClickHandler);

        data.pressPosition = data.position;
        data.pointerPress = newPointerPress;
        data.rawPointerPress = currentObject;
    }

    private void ProcessRelease(PointerEventData data)
    {
        GameObject pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentObject);
        ExecuteEvents.Execute(data.pointerPress, data, ExecuteEvents.pointerUpHandler);

        eventSystem.SetSelectedGameObject(null);

        data.pressPosition = Vector2.zero;
        data.pointerPress = null;
        data.rawPointerPress = null;
    }
}
