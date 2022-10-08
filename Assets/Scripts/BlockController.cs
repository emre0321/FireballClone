using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlockSpawnData
{
    public int blockPoolSize;
    public BlockModel blockPrefab;
    public BlockModel referanceFirstBlock;
    public float blocksHorizontalOffset;
    public float blocksMinYPositionValue;
    public float blocksMaxYPositionValue;
    public float previousBlockMaxYDifferance;

}

public class BlockController : MonoBehaviour
{
    [SerializeField] BlockSpawnData blockSpawnData;

    void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        BlockCreator.GetSingleton().Initialize(blockSpawnData);
    }

}
