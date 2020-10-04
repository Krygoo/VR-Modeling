using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshData
{
    public List<Vector3> vertices = new List<Vector3>();
    public List<int> triangles = new List<int>();
    public List<Vector2> uv = new List<Vector2>();

    public List<Vector3> collisionVertices = new List<Vector3>();
    public List<int> collisionTriangles = new List<int>();

    public bool useCollisions = true;

    public MeshData() { }

    public void AddQuadTriangles()
    {
        triangles.Add(vertices.Count - 4);
        triangles.Add(vertices.Count - 3);
        triangles.Add(vertices.Count - 2);

        triangles.Add(vertices.Count - 4);
        triangles.Add(vertices.Count - 2);
        triangles.Add(vertices.Count - 1);

        if (useCollisions) {
            collisionTriangles.Add(collisionVertices.Count - 4);
            collisionTriangles.Add(collisionVertices.Count - 3);
            collisionTriangles.Add(collisionVertices.Count - 2);

            collisionTriangles.Add(collisionVertices.Count - 4);
            collisionTriangles.Add(collisionVertices.Count - 2);
            collisionTriangles.Add(collisionVertices.Count - 1);
        }
    }

    public void AddVertex(Vector3 vertex)
    {
        vertices.Add(vertex);
 
        if (useCollisions)
        {
            collisionVertices.Add(vertex);
        }

    }
}