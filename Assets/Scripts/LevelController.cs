using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    GameObject ball;

    [SerializeField]
    Vector3 ballPosition;

    public void SpawnBall()
    {
        // todo get from the ball pool
        Instantiate(ball, ballPosition, Quaternion.identity);
    }
}
