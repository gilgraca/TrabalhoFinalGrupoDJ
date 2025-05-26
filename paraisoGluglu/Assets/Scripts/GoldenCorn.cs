using UnityEngine;

public class GoldenCorn : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0, 100, 0); // degrees per second

    void Update()
    {
        // Rotate object around its local axes at rotationSpeed degrees/second
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
