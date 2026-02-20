using DG.Tweening;
using NUnit.Framework.Internal.Filters;
using UnityEngine;
using UnityEngine.Rendering;

public class GolfBall : MonoBehaviour
{
    public float forceMultiplier = 1;
    public float maxForce = 10f;
    protected Rigidbody2D rb;

    bool launching;
    bool launched;

    bool win;
    bool won;

    bool destroying;

    [SerializeField]
    private LineRenderer trajectoryRenderer;

    private SpriteRenderer SpriteRenderer => GetComponent<SpriteRenderer>();

    private int trajectoryPoints = 10;

    public const float stopVelocityMagnitude = 0.1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    private void OnEnable()
    {
        trajectoryRenderer.enabled = launching;
    }

    private void OnDisable()
    {
        trajectoryRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (launched && rb.linearVelocity.magnitude < stopVelocityMagnitude && !destroying)
        {
            //destroying = true;
            //DestroyBall();
            rb.linearVelocity = Vector2.zero;
            launched = false;
            SpriteRenderer.color = Color.white;
        }

        if (win && !won && !launched)
        {
            won = true;
            OnWin();
            GameManager.Instance.Win();
        }
    }

    private void FixedUpdate()
    {
        if (rb.linearVelocity.magnitude > Mathf.Epsilon)
        {
            BounceOffCameraBoundaries();
        }
    }

    void BounceOffCameraBoundaries()
    {
        Vector2 pos = transform.position;
        Vector2 velocity = rb.linearVelocity;

        if (pos.x < -Camera.main.orthographicSize * Camera.main.aspect)
        {
            pos.x = -Camera.main.orthographicSize * Camera.main.aspect;
            velocity.x = -velocity.x;
        }
        else if (pos.x > Camera.main.orthographicSize * Camera.main.aspect)
        {
            pos.x = Camera.main.orthographicSize * Camera.main.aspect;
            velocity.x = -velocity.x;
        }

        if (pos.y < -Camera.main.orthographicSize)
        {
            pos.y = -Camera.main.orthographicSize;
            velocity.y = -velocity.y;
        }
        else if (pos.y > Camera.main.orthographicSize)
        {
            pos.y = Camera.main.orthographicSize;
            velocity.y = -velocity.y;
        }

        transform.position = pos;
        rb.linearVelocity = velocity;
    }

    public void Launching(Vector2 direction)
    {
        if (launched) return;
        launching = true;
        DrawTrajectory(direction);
    }

    public void Launch(Vector2 direction)
    {
        if (launched) return;

        if (direction == Vector2.zero) return;

        //rb.AddForce(direction, ForceMode2D.Impulse);
        direction = Vector2.ClampMagnitude(direction, maxForce);
        rb.linearVelocity = direction * forceMultiplier;
        launched = true;
        trajectoryRenderer.enabled = false;
        SpriteRenderer.color = new Color(.8f, .8f, .8f, 1f);

        GameManager.AddStroke();
    }

    void DrawTrajectory(Vector2 direction)
    {
        if (trajectoryRenderer == null) return;
        
        trajectoryRenderer.enabled = true;

        Vector2 initialVelocity = Vector2.ClampMagnitude(direction, maxForce);
        float initialVelocityX = initialVelocity.x;
        float initialVelocityY = initialVelocity.y;

        float timeStep = 0.1f;
        Vector3[] points = new Vector3[trajectoryPoints];

        for (int i = 0; i < trajectoryPoints; i++)
        {
            float time = i * timeStep;

            float posX = initialVelocityX * time;
            float posY = initialVelocityY * time;

            points[i] = transform.position  + new Vector3(posX, posY, 0f);
        }

        trajectoryRenderer.positionCount = points.Length;
        trajectoryRenderer.SetPositions(points);

    }

    public bool IsLaunched()
    {
        return launched;
    }

    public void SetWin()
    {
        win = true;
    }

    void OnWin()
    {
        transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack);

        SpriteRenderer.DOFade(0, 0.5f).onComplete = () =>
        {
            gameObject.SetActive(false);
        };
    }

    protected void DestroyBall()
    {

    }
}
