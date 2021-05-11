using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    // Unity Events
    public delegate void CreateBallAction();
    public static event ThrowController.ThrowContollerAction BallCreateListener;
    public static event EndGameUIController.EndGameAction EndGameListener;

    public static LevelController Instance;

    [Header("Basketball Hoop")]
    [SerializeField]
    GameObject basketballHoop;

    [Header("Ball")]
    [SerializeField]
    GameObject ballPrefab;

    [SerializeField]
    Vector3 spanwnPosition;

    [Header("Scriptable Objects")]
    [SerializeField]
    Ball defaultBall;

    [SerializeField]
    Ball specialBall1;
    
    [SerializeField]
    Ball specialBall2;

    // Dictionaries
    private Dictionary<int, int> ballNumbers;
    private Dictionary<int, float> basketballHoopPositions;
    private Dictionary<int, int> scorePerBasket;

    private int levelNumber;
    private int ballsRemaining;
    private WaitForSeconds delay;
    private float spawnDelay = 1;

    public int BallsRemaining { get => ballsRemaining; set => ballsRemaining = value; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        InitializeDictionaries();
        SetLevelStatus();
    }

    private void OnEnable()
    {
        ThrowController.BallThrowListener += OnBallThrowListener;
    }

    private void OnDisable()
    {
        ThrowController.BallThrowListener -= OnBallThrowListener;
    }

    private void OnBallThrowListener()
    {
        --ballsRemaining;

        if(ballsRemaining > 0)
        { 
            // If the last created ball is thrown, create a new ball.
            StartCoroutine(SpawnBall(delay));
        }
        else 
        {
            EndGameListener?.Invoke(); // not working !!
            GameManager.Instance.IsPlaying = false;
        }
    }

    private void Start()
    {
        GameManager.Instance.Score = 0;
        delay = new WaitForSeconds(spawnDelay);
        StartCoroutine(SpawnBall(new WaitForSeconds(0)));
    }

    private void InitializeDictionaries()
    {
        ballNumbers = new Dictionary<int, int>()
        {
            { 1, 20 }, { 2, 15 }, { 3, 10 }, { 4, 15 }, { 5, 20 },
            { 6, 15 }, { 7, 10 }, { 8, 15 }, { 9, 20}, { 10, 15}
        };

        scorePerBasket = new Dictionary<int, int>()
        {
            { 1, 1}, { 2, 2}, { 3, 3}, { 4, 2}, { 5, 1 },
            { 6, 2}, { 7, 3}, { 8, 2 }, { 9, 2}, { 10, 3 }
        };

        basketballHoopPositions = new Dictionary<int, float>()
        {
            { 1, 7.5f }, { 2, 7.5f }, { 3, 10}, { 4, 10 }, { 5, 12.5f },
            { 6, 12.5f }, { 7, 15 }, { 8, 15 }, { 9, 17.5f }, { 10, 17.5f  }
        };      

    }

    private void SetLevelStatus()
    {
        levelNumber = GetLevelNumber();

        ballsRemaining = ballNumbers[levelNumber];

        float zPosition = basketballHoopPositions[levelNumber];

        SetHoopPosition(zPosition);
    }

    private void SetHoopPosition(float zPos)
    {
        Vector3 currentPos = basketballHoop.transform.position;
        basketballHoop.transform.position = new Vector3(currentPos.x, currentPos.y, zPos);
    }

    public IEnumerator SpawnBall(WaitForSeconds spawnDelay)
    {
        yield return spawnDelay;

        if (!GameManager.Instance.IsThrowingBallExist) 
        {
            GameManager.Instance.IsThrowingBallExist = true;

            GameObject newBall = BallPooling.Instance.GetNewBall();

            Rigidbody ballRb = newBall.GetComponent<Rigidbody>();

            BallController ballController = newBall.GetComponent<BallController>();

            Ball ball = GetBallProperties();
            ballController.SetBallProperties(ball);

            BallCreateListener?.Invoke(ballRb);
        }
    }

    private Ball GetBallProperties()
    {
        if (levelNumber == 3)
            return specialBall1;

        if (levelNumber == 7)
            return specialBall2;

        return defaultBall;
    }

    public int GetScorPerBasketValue()
    {
        return scorePerBasket[levelNumber];
    }

    public int GetLevelNumber()
    {
        int currentlevel = PlayerPrefs.GetInt("LevelNumber");

        if (currentlevel == 0 || currentlevel > 10)
        {
            currentlevel = 1;
            PlayerPrefs.SetInt("LevelNumber", currentlevel);
        }

        return currentlevel;
    }

}
