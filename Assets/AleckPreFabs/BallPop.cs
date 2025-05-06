using UnityEngine;

public class BallPopper : MonoBehaviour
{
    public Rigidbody2D ballRigidbody;
    public float popForce = 10f;
    public float popRange = 1.5f;
    public KeyCode pushKey = KeyCode.None;

    void Update()
    {
        if (Input.GetKeyDown(pushKey) && ballRigidbody != null)
        {
            float distance = Vector2.Distance(transform.position, ballRigidbody.position);
            if (distance <= popRange)
            {
                ballRigidbody.AddForce(Vector2.up * popForce, ForceMode2D.Impulse);
            }
        }
    }
}
