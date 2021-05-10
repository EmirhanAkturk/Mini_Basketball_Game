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
        // increase score
        score += scorePerBasket;

        //update text
        scoreValueText.text = score.ToString();
    }

    private void OnBallThrowListener()
    {
        --ballsRemaining;
        ballsRemainingValueText.text = ballsRemaining.ToString();

        Debug.Log(ballsRemaining);
        if (ballsRemaining == 0)
        {
            GameManager.Instance.Score = score;
            //gameObject.SetActive(false);
        }
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
