//This class is on the main camera to follow player.
//You may optimize it on SetPosition section and
//Write a proper way to update blocks positions on the map to make it an infite gameplay.

using UnityEngine;

public class PlayerFollower : MonoBehaviour {

    [SerializeField] Transform Target;
    [SerializeField] float TransformLerpSpeed;
    [SerializeField] Vector3 Offset;

    private void LateUpdate()
    {
        //if (GameController.CurrentGameState != GameStates.Gameplay)
        //    return;

        transform.position = Vector3.Lerp(transform.position, Target.transform.position + Offset , Time.deltaTime * TransformLerpSpeed);

    }




}
