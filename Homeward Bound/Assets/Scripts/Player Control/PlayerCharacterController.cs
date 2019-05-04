using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{

    public static Transform PlayerCharacterTransform { get; private set; }

    [SerializeField]
    private KeyCode movementKey;

    [SerializeField]
    private float movementSpeed = 4;

    private Animator characterAnimator;

    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
        characterAnimator = GetComponentInChildren<Animator>();
        PlayerCharacterTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(movementKey) && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector2 mousePosition = mainCam.ScreenToWorldPoint(Input.mousePosition);

            characterAnimator.SetBool("Running", false);

            if (mousePosition.x < transform.position.x)
            {
                transform.localEulerAngles = new Vector3(0, 180, 0);
            }
            else
            {
                transform.localEulerAngles = new Vector3(0, 0, 0);
            }

            StopAllCoroutines();
            StartCoroutine(MoveAgent(mousePosition));
        }
    }

    /*
     * Lerps the player Character from their starting position to a target position
     */
    private IEnumerator MoveAgent(Vector2 targetLocation)
    {
        Vector2 startPos = transform.position;
        //if we arent moving anywhere, return
        if (targetLocation.x != startPos.x)
        {

            Vector2 endPos = targetLocation.SetY(startPos.y);
            float startTime = Time.time;
            float journeyLength = Vector2.Distance(startPos, endPos);

            float distCovered;
            float fracCompleted;

            //start run animation
            characterAnimator.SetBool("Running", true);
            do
            {
                distCovered = (Time.time - startTime) * movementSpeed;
                fracCompleted = distCovered / journeyLength;

                transform.position = Vector2.Lerp(startPos, endPos, fracCompleted);
                yield return new WaitForFixedUpdate();
            } while (fracCompleted < 1);

            //stop animation
            characterAnimator.SetBool("Running", false);
        }
        yield return null;
    }


}
