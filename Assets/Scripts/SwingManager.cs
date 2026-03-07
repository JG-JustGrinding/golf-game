using UnityEngine;

public class SwingManager : MonoBehaviour
{
    [SerializeField]
    private GolfBall currentBall;

    Vector2 startPos;
    Vector2 endPos;
    Vector2 Direction => startPos - endPos;

    public GameObject startObject;
    public GameObject endObject;

    public SwingMode swingMode = SwingMode.Global;

    public enum SwingMode
    {
        Global, // Can be used anywhere on the screen
        Selection, // Player must click on the ball to start the swing
    }

    const float directionMultiplier = 0.05f; // affects how far the player has to drag to reach max force (lower multiplier = more drag needed)

    const float launchThresshold = 0.5f; // minimum drag distance to launch the ball

    void Start()
    {
        disableEffectObjects();
    }

    void Update()
    {
        if (GameManager.Instance.won)
        {
            return;
        }

        if (Time.timeScale == 0)
        {
            return;
        }

        if (swingMode == SwingMode.Global)
        {
            if (Input.GetMouseButtonDown(0))
            {
                startPos = Input.mousePosition;
                Vector2 startObjectPos = Camera.main.ScreenToWorldPoint(startPos);
                startObject.transform.position = startObjectPos;
                startObject.SetActive(true);
            }

            if (Input.GetMouseButton(0))
            {
                endPos = Input.mousePosition;
                currentBall.Launching(Direction * directionMultiplier);
                Vector2 endObjectPos = Camera.main.ScreenToWorldPoint(endPos);
                endObject.transform.position = endObjectPos;
                endObject.SetActive(Direction.magnitude * directionMultiplier >= launchThresshold);
            }

            else if (Input.GetMouseButtonUp(0))
            {
                endPos = Input.mousePosition;

                if (Direction.magnitude * directionMultiplier >= launchThresshold)
                {
                    currentBall.Launch(Direction * directionMultiplier);
                } else
                {
                    currentBall.Launch(Vector2.zero);
                }

                disableEffectObjects();
            }
        }
        else if (swingMode == SwingMode.Selection)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Collider2D hitCollider = Physics2D.OverlapPoint(mouseWorldPos);
                GolfBall golfBall = hitCollider != null ? hitCollider.GetComponent<GolfBall>() : null;
                if (hitCollider != null && golfBall != null)
                {
                    currentBall = golfBall;
                    startPos = Input.mousePosition;
                }

                disableEffectObjects();
            }
            else if (Input.GetMouseButtonUp(0) && currentBall != null)
            {
                endPos = Input.mousePosition;
                currentBall.Launch(Direction * directionMultiplier);
                disableEffectObjects();
            }

            if (Input.GetMouseButton(0) && currentBall != null)
            {
                endPos = Input.mousePosition;
                currentBall.Launching(Direction * directionMultiplier);
            }
        }
    }

    void disableEffectObjects()
    {
        startObject.SetActive(false);
        endObject.SetActive(false);
    }
}
