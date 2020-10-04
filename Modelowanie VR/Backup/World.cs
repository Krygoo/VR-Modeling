using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public Dictionary<WorldPos, Chunk> chunks = new Dictionary<WorldPos, Chunk>();
    public GameObject chunkPrefab;

    void Start()
    {

        for (int x = -2; x < 2; x++)
        {
            for (int y = -1; y < 1; y++)
            {
                for (int z = -1; z < 1; z++)
                {
                    CreateChunk(x * 16, y * 16, z * 16);
                }
            }
        }
    }

    private void Update()
    {
        Vector3 mousePos = Input.mousePosition; //Pozycja kursora myszy
        mousePos.z = 15; // Oddalenie względem kamery
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Debug.Log(mousePos);
        if (Input.GetMouseButton(0))
        {
            SetBlock(Mathf.FloorToInt(mousePos.x), Mathf.FloorToInt(mousePos.y), Mathf.FloorToInt(mousePos.z), new Block());
        }
    }

    public void CreateChunk(int x, int y, int z)
    {
        WorldPos worldPos = new WorldPos(x, y, z);

        //Instantiate the chunk at the coordinates using the chunk prefab
        GameObject newChunkObject = Instantiate(
                        chunkPrefab, new Vector3(x, y, z),
                        Quaternion.identity
                    ) as GameObject;

        Chunk newChunk = newChunkObject.GetComponent<Chunk>();

        newChunk.pos = worldPos;
        newChunk.world = this;

        //Add it to the chunks dictionary with the position as the key
        chunks.Add(worldPos, newChunk);

        for (int xi = 0; xi < 16; xi++)
        {
            for (int yi = 0; yi < 16; yi++)
            {
                for (int zi = 0; zi < 16; zi++)
                    if (yi <= 7)
                    {
                        SetBlock(x + xi, y + yi, z + zi, new Block());
                    }
                    else
                    {
                        SetBlock(x + xi, y + yi, z + zi, new BlockAir());
                    }
                
            }
        }
    }

    public void DestroyChunk(int x, int y, int z)
    {
        Chunk chunk = null;
        if (chunks.TryGetValue(new WorldPos(x, y, z), out chunk))
        {
            Object.Destroy(chunk.gameObject);
            chunks.Remove(new WorldPos(x, y, z));
        }
    }

    public Chunk GetChunk(int x, int y, int z)
    {
        WorldPos pos = new WorldPos();
        float multiple = Chunk.chunkSize;
        pos.x = Mathf.FloorToInt(x / multiple) * Chunk.chunkSize;
        pos.y = Mathf.FloorToInt(y / multiple) * Chunk.chunkSize;
        pos.z = Mathf.FloorToInt(z / multiple) * Chunk.chunkSize;

        Chunk chunk = null;

        chunks.TryGetValue(pos, out chunk);

        return chunk;
    }

    public Block GetBlock(int x, int y, int z)
    {
        Chunk chunk = GetChunk(x, y, z);

        if (chunk != null)
        {
            Block block = chunk.GetBlock(
                x - chunk.pos.x,
                y - chunk.pos.y,
                z - chunk.pos.z);

            return block;
        }
        else
        {
            return new BlockAir();
        }
    }

    public void SetBlock(int x, int y, int z, Block block)
    {
        Chunk chunk = GetChunk(x, y, z);

        if (chunk != null)
        {
            chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, block);
            chunk.update = true;
        }
    }
}
