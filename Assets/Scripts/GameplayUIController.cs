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
    private int scorePerBasket;

    private void OnEnable()
    {
        BallController.BallFallListener += OnBallFallListener;
        BasketCheckher.BasketListener += OnBasketListener;
    }

    private void OnDisable()
    {
        BallController.BallFallListener -= OnBallFallListener;
        BasketCheckher.BasketListener -= OnBasketListener;
    }

    private void OnBasketListener()
    {
        // increase score
        GameManager.Instance.Score += scorePerBasket;

        //update text
        scoreValueText.text = GameManager.Instance.Score.ToString();
    }

    private void OnBallFallListener()
    {
        --ballsRemaining;
        ballsRemainingValueText.text = ballsRemaining.ToString();
    }

    private void Start()
    {
        // Get level values
        scorePerBasket = LevelController.Instance.GetScorPerBasketValue();
        ballsRemaining = LevelController.Instance.BallsRemaining;

        // Initialize texts
        ballsRemainingValueText.text = ballsRemaining.ToString();
        scoreValueText.text = score.ToString();
    }
}
