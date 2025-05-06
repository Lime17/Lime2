using UnityEngine;

public class ObjectSwap : MonoBehaviour
{
    [Header("Swap Target")]
    public GameObject ball;

    [Header("Control")]
    public KeyCode swapKey = KeyCode.LeftShift; // This line is REQUIRED

    void Update()
    {
        if (Input.GetKeyDown(swapKey) && ball != null)
        {
            SwapPositions();
        }
    }

    public void SwapPositions()
    {
        Vector3 tempPosition = ball.transform.position;
        ball.transform.position = transform.position;
        transform.position = tempPosition;
    }
}
