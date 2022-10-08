using UnityEngine;
using System.Collections;


//In this section, you have to edit OnPointerDown and OnPointerUp sections to make the game behave in a proper way using hJoint
//Hint: You may want to Destroy and recreate the hinge Joint on the object. For a beautiful gameplay experience, joint would created after a little while (0.2 seconds f.e.) to create mechanical challege for the player
//And also create fixed update to make score calculated real time properly.
//Update FindRelativePosForHingeJoint to calculate the position for you rope to connect dynamically
//You may add up new functions into this class to make it look more understandable and cosmetically great.

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private HingeJoint hJoint;
    [SerializeField]
    private LineRenderer lRenderer;
    [SerializeField]
    private Rigidbody playerRigidbody;

    [SerializeField]
    private float ForcePower;

    [SerializeField]
    private GUIController guiController;

    private float score;
    private float fakeScore = 0;

    private IEnumerator HingeJoingCr;

    public void FindRelativePosForHingeJoint(Vector3 blockPosition)
    {
        //Update the block position on this line in a proper way to Find Relative position for our blockPosition
        if(HingeJoingCr == null)
        {
            HingeJoingCr = GenerateHingeJoint(blockPosition);
            StartCoroutine(HingeJoingCr);
        }
        
    }

    public IEnumerator GenerateHingeJoint(Vector3 blockPosition)
    {
        yield return new WaitForSeconds(0.01f);
        if (hJoint == null)
            hJoint = gameObject.AddComponent<HingeJoint>();
        playerRigidbody.AddForce(new Vector3(0, 0, 0.001f));
        Vector3 inversedPoint = transform.InverseTransformPoint(blockPosition);
        hJoint.anchor = inversedPoint;
        SetLineRendererPosition(inversedPoint, true);
        HingeJoingCr = null;

    }

    public void SetBallFall()
    {
        if (hJoint)
            Destroy(hJoint);
        SetLineRendererPosition(Vector3.zero, false);
        AddForceToPlayer();
    }

    public void SetLineRendererPosition(Vector3 finalPos, bool isOpen)
    {
        lRenderer.SetPosition(1, finalPos);
        lRenderer.enabled = isOpen ? true : false;
    }

    public void AddForceToPlayer()
    {
        Vector3 forceDir = playerRigidbody.velocity;
        playerRigidbody.AddForce(forceDir * ForcePower, ForceMode.Impulse);
    }

    public void PointerDown()
    {
        Debug.Log("Pointer Down");
        //This function works once when player holds on the screen
        //FILL the behaviour here when player holds on the screen. You may or not call other functions you create here or just fill it here
        if (GameController.CurrentGameState == GameStates.MainMenu)
            GameController.ChangeGameState(GameStates.Gameplay);

        Transform relativeBlock = BlockCreator.GetSingleton().GetRelativeBlock(transform.position.z);
        FindRelativePosForHingeJoint(relativeBlock.position);
    }

    public void PointerUp()
    {
        Debug.Log("Pointer Up");
        //This function works once when player takes his/her finger off the screen
        //Fill the behaviour when player stops holding the finger on the screen.
        SetBallFall();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Block") && GameController.CurrentGameState == GameStates.Gameplay)
        {
            PointerUp(); //Finishes the game here to stoping holding behaviour
            GameController.ChangeGameState(GameStates.LevelFailed);
            //If you know a more modular way to update UI, change the code below
            GameController.singleton.SetScore(score);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Point"))
        {
            if (Vector3.Distance(transform.position, other.gameObject.transform.position) < .5f)
            {
                score += 10f;
            }
            else
            {
                score += 5f;
            }
            other.gameObject.SetActive(false);
        }
    }
    private void FixedUpdate()
    {
        //Score doesn't set properly since it always tend to update the score. Make a proper way to update the score as player advances
        SetScore();
    }
    public void SetScore()
    {
        if (score < fakeScore)
        {
            score = fakeScore;
        }
        fakeScore += playerRigidbody.velocity.z * Time.fixedDeltaTime * 0.1f;
        guiController.realtimeScoreText.text = score.ToString("0.00");
    }
}
