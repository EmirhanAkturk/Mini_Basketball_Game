using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    // Unity Events
    public static event ThrowController.ThrowContollerActions ballCreateListener;
    public delegate void LevelControllerAcions();

    public static LevelController Instance;

    [SerializeField]
    GameObject ballPrefab;

    [Header("Scriptable Objects")]
    [SerializeField]
    Ball defaultBall;

    [SerializeField]
    Ball specialBall;

    private WaitForSeconds delay;
    private float spawnDelay = 1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void OnEnable()
    {
        ThrowController.ballThrowListener += OnBallThrowListener;
    }

    private void OnDisable()
    {
        ThrowController.ballThrowListener -= OnBallThrowListener;
    }

    private void OnBallThrowListener()
    {
        // If the last created ball is thrown, create a new ball.
        StartCoroutine(SpawnBall(delay));
    }

    private void Start()
    {
        delay = new WaitForSeconds(spawnDelay);
        
        //StartCoroutine( SpawnBall( new WaitForSeconds(0) ) );
    }

    public IEnumerator SpawnBall(WaitForSeconds spawnDelay)
    {
        yield return spawnDelay;

        if (!GameManager.Instance.IsThrowingBallExist) 
        {
            GameManager.Instance.IsThrowingBallExist = true;

            // todo get from the ball pool
            GameObject newBall = Instantiate(ballPrefab, ballPrefab.transform.position, ballPrefab.transform.rotation);

            Rigidbody ballRb = newBall.GetComponent<Rigidbody>();

            BallController ballController = newBall.GetComponent<BallController>();
            ballController.SetBallProperties(defaultBall); // todo control which ball

            ballCreateListener?.Invoke(ballRb);
        }
    }
}
