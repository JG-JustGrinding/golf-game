using UnityEngine;

public class GolfHole : MonoBehaviour
{
    public float radius = 0.5f;
    public bool autoCalculateRadius = true;
    public float rollInForce = 1f; // holes will pull the ball in with a force simulating the rolling effect
    public LayerMask ballLayer;

    Collider2D[] gameObjects;

    void Start()
    {
        gameObjects = new Collider2D[0];
    }

    void FixedUpdate()
    {
        foreach (Collider2D collider in gameObjects)
        {
            GolfBall golfBall = collider.GetComponent<GolfBall>();

            if (golfBall)
            {
                if (golfBall.IsLaunched())
                {
                    if (collider.TryGetComponent<Rigidbody2D>(out var ballRb))
                    {
                        Vector2 directionToHole = (transform.position - collider.transform.position).normalized;
                        float distance = Vector2.Distance(transform.position, collider.transform.position);
                        float force = rollInForce;

                        if (distance < radius * .5)
                        {
                            // decrease force as the ball gets closer to the hole to prevent the ball from overshooting and bouncing around the hole
                            force *= distance / (radius * .5f);
                        }


                        ballRb.AddForce(directionToHole * force, ForceMode2D.Force);
                    }
                }
            }
        }
    }

    private void Update()
    {
        gameObjects = Physics2D.OverlapCircleAll(transform.position, radius, ballLayer);

        foreach (Collider2D collider in gameObjects)
        {
            if (collider.TryGetComponent<GolfBall>(out var golfBall))
            {
                float distance = Vector2.Distance(transform.position, collider.transform.position);

                if (distance <= radius * 0.5 && !golfBall.IsLaunched())
                {
                    golfBall.SetWin();
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    void CalculateRadius()
    {
        if (autoCalculateRadius)
        {
            radius = radius * transform.lossyScale.x / 2;
        }
    }

    private void OnValidate()
    {
        CalculateRadius();
    }
}
