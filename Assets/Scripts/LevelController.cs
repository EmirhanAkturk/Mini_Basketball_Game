using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static event ThrowController.ThrowContollerActions ballCreateListener;

    public static LevelController Instance;

    [SerializeField]
    GameObject ball;

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
            GameObject newBall = Instantiate(ball, ball.transform.position, Quaternion.identity);

            Rigidbody ballRb = newBall.GetComponent<Rigidbody>();
            //ballRb.isKinematic = true;

            ballCreateListener?.Invoke(ballRb);
        }
    }
}
