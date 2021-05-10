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
    private int levelNumber;

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
        IncreaseScore();
        scoreValueText.text = score.ToString();
    }

    private void OnBallThrowListener()
    {
        --ballsRemaining;
        ballsRemainingValueText.text = ballsRemaining.ToString();

        Debug.Log(ballsRemaining);
        if (ballsRemaining == 0)
        {
            Debug.Log("***" + ballsRemaining);

            GameManager.Instance.Score = score;
        }
    }

    private void Start()
    {
        levelNumber = LevelController.Instance.GetLevelNumber();

        ballsRemaining = LevelController.Instance.BallsRemaining;
        ballsRemainingValueText.text = ballsRemaining.ToString();
        scoreValueText.text = score.ToString();
    }


    private void IncreaseScore()
    {
        if (levelNumber == 3)
            score += 2;
        else if (levelNumber == 7)
            score += 3;
        else
            ++score;
    }

}
