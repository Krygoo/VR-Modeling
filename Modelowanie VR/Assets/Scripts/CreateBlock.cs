using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class CreateBlock : MonoBehaviour
{
    private World world;
    BlockPointer pointer;
    int color;
    BrushManager brushManager;

    private void Start()
    {
        world = FindObjectOfType<World>();
        pointer = FindObjectOfType<BlockPointer>();
        brushManager = GetComponent<BrushManager>();
    }
    // Update is called once per frame
    void Update()
    {
        float triggerValue = SteamVR_Actions._default.CreateBlock.GetAxis(SteamVR_Input_Sources.Any);
        if (triggerValue > 0 && !SteamVR_Actions._default.Delete.GetState(SteamVR_Input_Sources.Any))
        {
            CreateBlocks(Vector3Int.FloorToInt(pointer.points[1]), triggerValue);
        }
    }
    //PRZETESTUJ WSZYSTKO!
    void CreateBlocks(Vector3Int position, float value)
    {
        int additional;
        if (value == 1)
            additional = 3;
        else if (value > 0.66)
            additional = 2;
        else if (value > 0.33)
            additional = 1;
        else
            additional = 0;
        for (int i = position.x - additional; i <= position.x + additional; i++)
        {
            for (int j = position.y - additional; j <= position.y + additional; j++)
            {
                for (int k = position.z - additional; k <= position.z + additional; k++)
                {
                    switch (brushManager.brushType)
                    {
                        case 0:
                            if (brushManager.brushMode == 1)
                            {
                                float distance = Vector3.Distance(new Vector3(i, j, k), position);
                                if (distance <= additional)
                                    world.SetBlock(i, j, k, new Block(color));
                            }
                            else world.SetBlock(i, j, k, new Block(color));
                            break;
                        case 1:
                            if ((position.x - i + position.y - j + position.z - k) % 2 == 0)
                            {
                                if (brushManager.brushMode == 1)
                                {
                                    float distance = Vector3.Distance(new Vector3(i, j, k), position);
                                    if (distance <= additional)
                                        world.SetBlock(i, j, k, new Block(color));
                                }
                                else world.SetBlock(i, j, k, new Block(color));
                            }
                            break;
                        case 2:
                            if (position.x == i || position.y == j || position.z == k)
                            {
                                if (brushManager.brushMode == 1)
                                {
                                    float distance = Vector3.Distance(new Vector3(i, j, k), position);
                                    if (distance <= additional)
                                        world.SetBlock(i, j, k, new Block(color));
                                }
                                else world.SetBlock(i, j, k, new Block(color));
                            }
                            break;
                    }
                    
                }
            }
        }
    }

    public void ChangeColor(int colorIndex)
    {
        color = colorIndex;
    }
}
