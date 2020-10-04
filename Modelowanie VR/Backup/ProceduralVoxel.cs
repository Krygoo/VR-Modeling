using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]

public class ProceduralVoxel : MonoBehaviour
{
    Mesh mesh;
    List<Vector3> vertices;
    List<int> triangles;
    int faces = 6;
    public float scale = 1;
    int vertCount, triCount;

    void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        vertices = new List<Vector3>();
        triangles = new List<int>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0)){
            GenerateVoxel(scale);
            UpdateMesh();
            vertCount = mesh.vertices.Length;
            triCount = mesh.triangles.Length;
            Debug.Log(vertCount);
            Debug.Log(triCount);
        }
    }

    void GenerateVoxel(float voxelScale)
    {
        Vector3 pos = Input.mousePosition; //Pozycja kursora myszy
        pos.z = 100; // Oddalenie względem kamery
        pos = Camera.main.ScreenToWorldPoint(pos);

        for (int i=0; i < faces; i++)
        {
            GenerateFace(i, voxelScale, pos);
        }
    }

    void GenerateFace(int face, float faceScale, Vector3 facePos)
    {
        vertices.AddRange(VoxelData.faceVertices(face, faceScale, facePos));

        triangles.Add(vertices.Count - 4);  //Dodawanie wierzcholkow pierwszego trojkata sciany
        triangles.Add(vertices.Count - 3);
        triangles.Add(vertices.Count - 2);

        triangles.Add(vertices.Count - 4);  //Dodawanie wierzcholkow drugiego trojkata sciany
        triangles.Add(vertices.Count - 2);
        triangles.Add(vertices.Count - 1);
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }
}
