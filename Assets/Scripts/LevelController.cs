﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    // Unity Events
    public static event ThrowController.ThrowContollerAction ballCreateListener;
    public delegate void CreateBallAction();

    public static LevelController Instance;

    [SerializeField]
    GameObject ballPrefab;

    [SerializeField]
    GameObject basketballHoop;

    [Header("Scriptable Objects")]
    [SerializeField]
    Ball defaultBall;

    [SerializeField]
    Ball specialBall1;
    
    [SerializeField]
    Ball specialBall2;

    private Dictionary<int, int> ballNumbers;
    private Dictionary<int, float> basketballHoopPositions;

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
    }

    private void OnEnable()
    {
        ThrowController.ballThrowListener1 += OnBallThrowListener;
    }

    private void OnDisable()
    {
        ThrowController.ballThrowListener1 -= OnBallThrowListener;
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
            GameManager.Instance.IsPlaying = false;
            // todo send event level is finish
        }

    }

    private void Start()
    {
        //reset score at the beginning of the level
        GameManager.Instance.Score = 0;

        delay = new WaitForSeconds(spawnDelay);

        InitializeDictionaries();

        //levelNumber = PlayerPrefs.GetInt("LevelNumber");
        levelNumber = GameManager.Instance.LevelNumber;

        Debug.Log(levelNumber);

        ballsRemaining = ballNumbers[levelNumber];
        float zPosition = basketballHoopPositions[levelNumber];

        SetHoopPosition(zPosition);
    }

    private void InitializeDictionaries()
    {
        ballNumbers = new Dictionary<int, int>()
        {
            { 1, 15 }, { 2, 10 }, { 3, 15 }, { 4, 10 }, { 5, 15 },
            { 6, 10 }, { 7, 15 }, { 8, 10 }, { 9, 15 }, { 10, 10 }
        };

        basketballHoopPositions = new Dictionary<int, float>()
        {
            { 1, 7.5f }, { 2, 7.5f }, { 3, 10}, { 4, 10 }, { 5, 12.5f },
            { 6, 12.5f }, { 7, 15 }, { 8, 15 }, { 9, 17.5f }, { 10, 17.5f  }
        };
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

            // todo get from the ball pool
            GameObject newBall = Instantiate(ballPrefab, ballPrefab.transform.position, ballPrefab.transform.rotation);

            Rigidbody ballRb = newBall.GetComponent<Rigidbody>();

            BallController ballController = newBall.GetComponent<BallController>();

            Ball ball = GetBall();
            ballController.SetBallProperties(ball);

            ballCreateListener?.Invoke(ballRb);
        }
    }

    private Ball GetBall()
    {
        if (levelNumber == 3)
            return specialBall1;

        if (levelNumber == 7)
            return specialBall2;

        return defaultBall;
    }
}
