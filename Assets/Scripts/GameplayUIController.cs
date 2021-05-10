using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameplayUIController : MonoBehaviour
{
    public delegate void RemainingBallAction();
    public delegate void BasketAction();

    [SerializeField]
    TMP_Text scoreValueText;
    
    [SerializeField]
    TMP_Text ballsRemainingValueText;

    private int ballsRemaining;
    private int score;

    private void OnEnable()
    {
        ThrowController.BallThrowListener1 += OnBallThrowListener;
        BasketCheckher.BasketListener += OnBasketListener;
    }

    private void OnDisable()
    {
        ThrowController.BallThrowListener1 -= OnBallThrowListener;
        BasketCheckher.BasketListener -= OnBasketListener;
    }

    private void OnBasketListener()
    {
        ++score;
        scoreValueText.text = score.ToString();
    }

    private void OnBallThrowListener()
    {
        --ballsRemaining;
        ballsRemainingValueText.text = ballsRemaining.ToString();

        if (ballsRemaining == 0)
        {
            Debug.Log("Ball remaining = 0");
            GameManager.Instance.Score = score;
        }
    }

    private void Start()
    {
        ballsRemaining = LevelController.Instance.BallsRemaining;
        ballsRemainingValueText.text = ballsRemaining.ToString();
        scoreValueText.text = score.ToString();
    }
}
