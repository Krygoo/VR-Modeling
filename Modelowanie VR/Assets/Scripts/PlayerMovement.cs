using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PlayerMovement : MonoBehaviour
{
    TeleportPointer pointer;
    Transform controller;
    float triggerValue;

    // Start is called before the first frame update
    void Start()
    {
        pointer = FindObjectOfType<TeleportPointer>();
        controller = this.gameObject.transform.GetChild(0).Find("Controller (left)");
    }

    // Update is called once per frame
    void Update()
    {
        triggerValue = SteamVR_Actions._default.MoveTrigger.GetAxis(SteamVR_Input_Sources.Any);
        if (triggerValue > 0)
            MovePlayer(triggerValue);

        if (SteamVR_Actions._default.MoveBool.GetStateDown(SteamVR_Input_Sources.Any))
        {
            pointer.render = true;
        }
        if(SteamVR_Actions._default.MoveBool.GetStateUp(SteamVR_Input_Sources.Any))
        {
            TeleportPlayer(pointer.position);
            pointer.render = false;
        }
    }

    void MovePlayer(float speed)
    {
        transform.Translate(controller.forward * speed * 10 * Time.deltaTime);
    }

    void TeleportPlayer(Vector3 position)
    {
        transform.position = position;
    }
}
