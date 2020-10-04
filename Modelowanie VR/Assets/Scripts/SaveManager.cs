using System;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    SaveData saveData;

    public void GetBlocks()
    {
        World world = FindObjectOfType<World>();
        foreach (var chunk in world.chunks)
        {
            if (!chunk.Value.IsEmpty())
            {
                for (int i = 0; i < 16; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        for (int k = 0; k < 16; k++)
                        {
                            Block blockToSave = chunk.Value.GetBlock(i, j, k);
                            if (!(blockToSave is BlockAir))
                            {
                                if (saveData.blocks == null)
                                    saveData.blocks = new List<BlockAttributes>();

                                var attributes = new BlockAttributes()
                                {
                                    x = chunk.Key.x + i,
                                    y = chunk.Key.y + j,
                                    z = chunk.Key.z + k,
                                    blockColor = blockToSave.color
                                };
                                saveData.blocks.Add(attributes);
                                /*
                                attributes.x = chunk.Key.x + i;
                                attributes.y = chunk.Key.y + j;
                                attributes.z = chunk.Key.z + k;
                                attributes.blockColor = blockToSave.color;
                                saveData.blocks.Add(attributes);
                                */
                            }
                        }
                    }
                }
            }
        }
    }

    public void Save(int index)
    {
        GetBlocks();
        //TODO: Zmienić nazwę save'a
        SerializationManager.Save(index.ToString(), saveData);
    }

    public void Load(int index)
    {
        //TODO: Zmienić path
        saveData = (SaveData)SerializationManager.Load(Application.persistentDataPath + "/saves/" + index.ToString() + ".save");
        if (saveData.blocks != null)
        {
            World world = FindObjectOfType<World>();
            ChunkLoader chunkLoader = FindObjectOfType<ChunkLoader>();
            foreach (var block in saveData.blocks)
            {
                if (world.GetChunk(block.x, block.y, block.z) == null)
                    world.CreateChunk(
                        Mathf.FloorToInt(block.x / (float)Chunk.chunkSize) * Chunk.chunkSize,
                        Mathf.FloorToInt(block.y / (float)Chunk.chunkSize) * Chunk.chunkSize,
                        Mathf.FloorToInt(block.z / (float)Chunk.chunkSize) * Chunk.chunkSize);
                world.SetBlock(block.x, block.y, block.z, new Block(block.blockColor));
            }
        }
    }
}
[Serializable]
public struct SaveData
{
    public List<BlockAttributes> blocks;
}

[Serializable]
public struct BlockAttributes
{
    public int x;
    public int y;
    public int z;
    public int blockColor;
}
