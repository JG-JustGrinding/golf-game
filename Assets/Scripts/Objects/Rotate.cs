using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float rotationSpeed = 50f;

    void Update()
    {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
