using System.Collections.Generic;
using UnityEngine;


//In this class, the map has been created.
//You have to edit GetRelativeBlock section to calculate current relative block to cast player rope to hold on
//Update Block Position section to make infinite map.
public class BlockCreator : MonoBehaviour
{

    private static BlockCreator singleton = null;


    [SerializeField] BlockSpawnData blockSpawnData;

    public List<BlockModel> blockPool = new List<BlockModel>(); // PRIVATE OLCAK


    private float lastHeightUpperBlock; // Y
    private int difficulty = 1;

    public static BlockCreator GetSingleton()
    {
        if (singleton == null)
        {
            singleton = new GameObject("_BlockCreator").AddComponent<BlockCreator>();
        }
        return singleton;
    }

    public void Initialize(BlockSpawnData data)
    {
        blockSpawnData = data;
        FillBlockPool();
        GenerateDefaultBlocks();
    }


    public void FillBlockPool()
    {
        //Instantiate blocks here
        GameObject tempBlocksParent = new GameObject("BLOCKS");

        for (int i = 0; i < blockSpawnData.blockPoolSize; i++)
        {
            BlockModel tempBlockModel = Instantiate(blockSpawnData.blockPrefab);
            tempBlockModel.Initialize();
            tempBlockModel.gameObject.SetActive(false);
            blockPool.Add(tempBlockModel);
            tempBlockModel.transform.SetParent(tempBlocksParent.transform);
        }
    }

    void Update()
    {
      
    }


    public Transform GetRelativeBlock(float playerPosZ)
    {
        for (int i = 0; i < blockPool.Count; i++)
        {
            float calculatedDistance = blockPool[i].transform.position.z - playerPosZ ;

            if (calculatedDistance < -3f)
            {
                UpdateBlockPosition(i);
            }

        }



        //You may need this type of getter to which block are we going to cast our rope into
        float minZOffset = -4.5f;
        float maxZOffset = -3f;
        for (int i = 0; i < blockPool.Count; i++)
        {
            float calculatedDistance = playerPosZ - blockPool[i].transform.position.z;

            if (calculatedDistance >= minZOffset && calculatedDistance < maxZOffset)
            {
               // Debug.Log("DISTANCE = " + calculatedDistance);
                Debug.Log("A", blockPool[i]);
                return blockPool[i].RopeGripPoint;
            }

        }
        Debug.Log("B" , blockPool[0]);
        return blockPool[0].RopeGripPoint;
    }

    public void UpdateBlockPosition(int blockIndex)
    {
        //Block Pool has been created. Find a proper way to make infite map when it is needed
        float tempRanY = UnityEngine.Random.Range(-blockSpawnData.previousBlockMaxYDifferance, blockSpawnData.previousBlockMaxYDifferance);
        BlockModel tempModel = blockPool[blockIndex];
        tempModel.transform.position = new Vector3(0,lastHeightUpperBlock + tempRanY, blockPool[blockPool.Count-1].transform.position.z + blockSpawnData.blocksHorizontalOffset);

        float finalizeYPos = tempModel.transform.position.y;

        if (finalizeYPos < blockSpawnData.blocksMinYPositionValue)
        {
            finalizeYPos = blockSpawnData.blocksMinYPositionValue;
            tempModel.transform.position = new Vector3(tempModel.transform.position.x, finalizeYPos, tempModel.transform.position.z);
        }
        else if (finalizeYPos > blockSpawnData.blocksMaxYPositionValue)
        {
            finalizeYPos = blockSpawnData.blocksMaxYPositionValue;
            tempModel.transform.position = new Vector3(tempModel.transform.position.x, finalizeYPos, tempModel.transform.position.z);
        }

        blockPool.Remove(tempModel);
        blockPool.Add(tempModel);

    }

    void GenerateDefaultBlocks()
    {
        int colorCounter = 0;

        for (int i = 0; i < blockPool.Count; i++)
        {
            blockPool[i].gameObject.SetActive(true);

            if (i == 0)
            {
                // First Block Position Set
                float tempRanY = UnityEngine.Random.Range(-blockSpawnData.previousBlockMaxYDifferance, 0);
                blockPool[i].transform.position = new Vector3(0, blockSpawnData.referanceFirstBlock.transform.position.y + tempRanY, blockSpawnData.blocksHorizontalOffset + blockSpawnData.referanceFirstBlock.transform.position.z);
            }
            else
            {
                // Other Blocks
                float tempRanY = UnityEngine.Random.Range(-blockSpawnData.previousBlockMaxYDifferance, blockSpawnData.previousBlockMaxYDifferance);
                blockPool[i].transform.position = new Vector3(0, blockPool[i-1].transform.position.y + tempRanY, blockPool[i - 1].transform.position.z + blockSpawnData.blocksHorizontalOffset);
                lastHeightUpperBlock = blockPool[i].transform.position.y;
            }

            // Position Limitation

            float finalizeYPos = blockPool[i].transform.position.y;

            if (finalizeYPos< blockSpawnData.blocksMinYPositionValue)
            {
                finalizeYPos = blockSpawnData.blocksMinYPositionValue;
                blockPool[i].transform.position = new Vector3(blockPool[i].transform.position.x,finalizeYPos , blockPool[i].transform.position.z);
            }
            else if (finalizeYPos > blockSpawnData.blocksMaxYPositionValue)
            {
                finalizeYPos = blockSpawnData.blocksMaxYPositionValue;
                blockPool[i].transform.position = new Vector3(blockPool[i].transform.position.x, finalizeYPos, blockPool[i].transform.position.z);
            }


            // Color Attach
            blockPool[i].SetBlockColorType(colorCounter);
            colorCounter++;
            if (colorCounter == 3)
                colorCounter = 0;

        }
    }
}
