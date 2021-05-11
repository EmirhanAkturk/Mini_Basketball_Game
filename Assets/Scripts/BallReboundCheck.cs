using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallReboundCheck : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        float ballZPos = other.gameObject.transform.position.z;

        if (ballZPos > transform.position.z)
        {
            BallController ballController = other.gameObject.GetComponent<BallController>();
            ballController.AddBackToPool(0);
        }
    }
}
