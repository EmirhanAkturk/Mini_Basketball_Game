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
        ThrowController.ballThrowListener1 += OnBallThrowListener;
        BasketCheckher.basketListener += OnBasketListiner;
    }


    private void OnDisable()
    {
        ThrowController.ballThrowListener1 -= OnBallThrowListener;
        BasketCheckher.basketListener -= OnBasketListiner;
    }


    private void OnBasketListiner()
    {
        ++score;
        scoreValueText.text = score.ToString();
    }

    private void OnBallThrowListener()
    {
        --ballsRemaining;
        ballsRemainingValueText.text = ballsRemaining.ToString();
    }

    private void Start()
    {
        ballsRemaining = LevelController.Instance.BallsRemaining;
        scoreValueText.text = score.ToString();
        ballsRemainingValueText.text = ballsRemaining.ToString();
    }
}
