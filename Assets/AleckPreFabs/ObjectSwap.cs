using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSwap : MonoBehaviour
{

public GameObject ball;

    // Update is called once per frame
   public void swap()
    {
        Vector3 ballPosition = ball.transform.position;
        Vector3 thisPosition = this.transform.position;
            ball.transform.position = thisPosition;
           this.transform.position = ballPosition;
    }
}