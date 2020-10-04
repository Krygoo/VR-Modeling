using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelPainter : MonoBehaviour
{
    [SerializeField] GameObject voxel;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Paint(10);
        }
    }

    public void Paint(float size)
    {
        float[] offset = { 0, 0, 0 };
        Vector3 pos = Input.mousePosition; //Pozycja kursora myszy
        pos.z = 100; // Oddalenie względem kamery
        pos = Camera.main.ScreenToWorldPoint(pos);
        //TODO: Po zmianie na prefabrykat edytuj inicjalizację offsetu (size/2 zamiast 1 jesli chodzi o rozmiar kostki)
        for (offset[0] = -size / 2; offset[0] <= size / 2; offset[0] = offset[0] + voxel.GetComponent<MeshRenderer>().bounds.size.x)
        {
            for (offset[1] = -size / 2; offset[1] <= size / 2; offset[1] = offset[1] + voxel.GetComponent<MeshRenderer>().bounds.size.y)
            {
                for (offset[2] = -size / 2; offset[2] <= size / 2; offset[2] = offset[2] + voxel.GetComponent<MeshRenderer>().bounds.size.z)
                {
                    Vector3 spawnPos = new Vector3(pos.x + offset[0], pos.y + offset[1], pos.z + offset[2]);
                    if (!Physics.CheckBox(spawnPos, voxel.GetComponent<Renderer>().bounds.size / 2))
                    {
                        Instantiate(voxel, spawnPos, Quaternion.identity);
                    }
                }
            }
        }
        Collider[] insideSphere = Physics.OverlapSphere(pos, 5);
        foreach(Collider collider in insideSphere)
        {
            collider.GetComponent<MeshRenderer>().enabled = true;
        }
        Collider[] insideBox = Physics.OverlapBox(pos, new Vector3(size / 2, size / 2, size / 2));
        foreach(Collider collider in insideBox)
        {
            if (collider.GetComponent<MeshRenderer>().enabled == false)
                Destroy(collider);
        }

        //TODO: Zmiana primitywa na prefabrykat
        //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //cube.transform.position = new Vector3(pos.x, pos.y, pos.z);
    }
}
