using UnityEngine;
public class MovingPlatform : MonoBehaviour
{
    public Transform point1;
    public Transform point2;
    public Transform point3;
    public float speed = 2f;
    public bool half = true;

    private Vector3 target;

    void Start()
    {
        if (point1 != null)
            target = point1.position;
    }

    void Update()
    {
        if (point1 == null || point2 == null) return;

        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            if (target == point1.position)
            {
                target = point2.position;
            }
            else if (target == point2.position)
            {
                target = (half == true) ? point3.position : point1.position;

                half = (half == true) ? false : true;
            }
            else if (target == point3.position)
            {
                target = point2.position;
            }
        }
    }
}