using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkLoader : MonoBehaviour
{
    int timer = 0;
    public World world;
    List<WorldPos> updateList = new List<WorldPos>();
    List<WorldPos> loadList = new List<WorldPos>();
    List<WorldPos> chunkPositions = new List<WorldPos>();
    // Start is called before the first frame update
    void Start()
    {
        chunkPositions = FillList(chunkPositions);
    }

    // Update is called once per frame
    void Update()
    {
        DeleteChunks();
        FindChunksToLoad();
        LoadAndRenderChunks();
    }

    List<WorldPos> FillList(List<WorldPos> list)
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                for (int k = 0; k < 6; k++)
                {
                    list.Add(new WorldPos(i, j, k));
                    if (i != 0) list.Add(new WorldPos(-i, j, k));
                    if (j != 0) list.Add(new WorldPos(i, -j, k));
                    if (k != 0) list.Add(new WorldPos(i, j, -k));
                    if (i != 0 && j != 0) list.Add(new WorldPos(-i, -j, k));
                    if (i != 0 && k != 0) list.Add(new WorldPos(-i, j, -k));
                    if (j != 0 && k != 0) list.Add(new WorldPos(i, -j, -k));
                    if (i != 0 && j != 0 && k != 0) list.Add(new WorldPos(-i, -j, -k));

                }
            }
        }
        return list;
    }

    void FindChunksToLoad()
    {
        WorldPos playerPos = new WorldPos(
        Mathf.FloorToInt(transform.position.x / Chunk.chunkSize) * Chunk.chunkSize,
        Mathf.FloorToInt(transform.position.y / Chunk.chunkSize) * Chunk.chunkSize,
        Mathf.FloorToInt(transform.position.z / Chunk.chunkSize) * Chunk.chunkSize
        );

        if (loadList.Count == 0)
        {
            for (int i = 0; i < chunkPositions.Count; i++)
            {
                WorldPos newChunkPos = new WorldPos(
                chunkPositions[i].x * Chunk.chunkSize + playerPos.x,
                chunkPositions[i].y * Chunk.chunkSize + playerPos.y,
                chunkPositions[i].z * Chunk.chunkSize + playerPos.z
                );

                Chunk newChunk = world.GetChunk(
                newChunkPos.x, newChunkPos.y, newChunkPos.z);

                if (newChunk != null
                && (newChunk.rendered || updateList.Contains(newChunkPos)))
                    continue;

                loadList.Add(new WorldPos(
                newChunkPos.x, newChunkPos.y, newChunkPos.z));
                return;
            }
        }
    }

    void LoadChunk(WorldPos pos)
    {
        for (int x = pos.x - Chunk.chunkSize; x <= pos.x + Chunk.chunkSize; x += Chunk.chunkSize)
        {
            for (int y = pos.y - Chunk.chunkSize; y <= pos.y + Chunk.chunkSize; y += Chunk.chunkSize)
            {
                for (int z = pos.z - Chunk.chunkSize; z <= pos.z + Chunk.chunkSize; z += Chunk.chunkSize)
                {
                    if (world.GetChunk(x, y, z) == null)
                        world.CreateChunk(x, y, z);
                }
            }
        }
        updateList.Add(pos);
    }

    void LoadAndRenderChunks()
    {
        for (int i = 0; i < 2; i++)
        {
            if (loadList.Count != 0)
            {
                LoadChunk(loadList[0]);
                loadList.RemoveAt(0);
            }
        }

        for (int i = 0; i < updateList.Count; i++)
        {
            Chunk chunk = world.GetChunk(updateList[0].x, updateList[0].y, updateList[0].z);
            if (chunk != null)
                chunk.update = true;
            updateList.RemoveAt(0);
        }
    }

    void DeleteChunks()
    {
        if (timer == 20)
        {
            var chunksToDelete = new List<WorldPos>();
            foreach (var chunk in world.chunks)
            {
                float distance = Vector3.Distance(
                new Vector3(chunk.Value.pos.x, chunk.Value.pos.y, chunk.Value.pos.z),
                new Vector3(transform.position.x, transform.position.y, transform.position.z));

                if (distance > 256 && chunk.Value.IsEmpty())
                    chunksToDelete.Add(chunk.Key);
            }

            foreach (var chunk in chunksToDelete)
                world.DestroyChunk(chunk.x, chunk.y, chunk.z);

            timer = 0;
        }

        timer++;
    }
}
