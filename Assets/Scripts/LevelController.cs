using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static event ThrowController.ThrowContollerActions ballCreateListener;

    public static LevelController Instance;

    [SerializeField]
    GameObject ball;

    [SerializeField]
    Vector3 ballPosition;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Start()
    {
        SpawnBall();
    }

    public void SpawnBall()
    {
        if (!GameManager.Instance.IsThrowingBallExist) 
        {
            GameManager.Instance.IsThrowingBallExist = true;

            // todo get from the ball pool
            ball = Instantiate(ball, ballPosition, Quaternion.identity);

            Rigidbody ballRb = ball.GetComponent<Rigidbody>();
            ballRb.isKinematic = true;

            ballCreateListener?.Invoke(ballRb);
        }
    }
}
