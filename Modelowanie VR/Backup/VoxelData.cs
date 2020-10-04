using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelData : MonoBehaviour
{
    public static Vector3[] vertices =
    {
        new Vector3(1,1,1),
        new Vector3(-1,1,1),
        new Vector3(1,-1,1),
        new Vector3(-1,-1,1),
        new Vector3(1,1,-1),
        new Vector3(-1,1,-1),
        new Vector3(1,-1,-1),
        new Vector3(-1,-1,-1)
    };

    public static int[][] faceQuads =
    {
        new int[] {0,1,3,2},    //sciana od strony dodatniego z
        new int[] {4,0,2,6},    //sciana od strony dodatniego x
        new int[] {5,4,6,7},    //sciana od strony ujemnego z
        new int[] {1,5,7,3},    //sciana od strony ujemnego x
        new int[] {1,0,4,5},    //sciana od strony dodatniego y
        new int[] {2,3,7,6},    //sciana od strony ujemnego y
    };

    public static Vector3[] faceVertices(int face, float scale, Vector3 pos)
    {
        Vector3[] fv = new Vector3[4];
        for(int i = 0; i < fv.Length; i++)
        {
            fv[i] = vertices[faceQuads[face][i]] * (scale * 0.5f) + pos;
        }
        return fv;
    }
}
