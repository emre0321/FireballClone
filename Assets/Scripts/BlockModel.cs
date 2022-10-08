using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockModel : MonoBehaviour
{
    [Header("ASSIGNMENTS")]
    [SerializeField] Transform TopBlock;
    [SerializeField] List<GameObject> TopBlocks;
    [SerializeField] Transform BottomBlock;
    [SerializeField] List<GameObject> BottomBlocks;
    [SerializeField] TargetModel Target;
    public Transform RopeGripPoint;

    [Header("DATA VALUES")]
    [SerializeField] float BlocksYOffset;
 


    public void Initialize()
    {
        SetBlocksYOffset();
        SetTarget(false);

    }

    public void SetBlocksYOffset()
    {
        TopBlock.transform.localPosition = new Vector3(0,BlocksYOffset/2,0);
        BottomBlock.transform.localPosition = new Vector3(0,-BlocksYOffset/2,0);
    }

    public void SetBlockColorType(int typeIndex)
    {
        //Close All Block Meshs
        for (int i = 0; i < TopBlocks.Count; i++)
        {
            TopBlocks[i].SetActive(false);
            BottomBlocks[i].SetActive(false);
        }

        switch (typeIndex)
        {
            case 0:
                TopBlocks[0].SetActive(true);
                BottomBlocks[0].SetActive(true);
                break;
            case 1:
                TopBlocks[1].SetActive(true);
                BottomBlocks[1].SetActive(true);

                break;
            case 2:
                TopBlocks[2].SetActive(true);
                BottomBlocks[2].SetActive(true);
                break;
        }



    }

    public void SetTarget(bool isOpen)
    {
        if (isOpen)
        {
            Target.gameObject.SetActive(true);
        }
        else
        {
            Target.gameObject.SetActive(false);
        }
    }

}
