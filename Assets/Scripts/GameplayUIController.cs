using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameplayUIController : MonoBehaviour
{
    public delegate void UpdateUIAction();
    public delegate void BasketAction();

    [SerializeField]
    GameObject gameplayPanel;

    [SerializeField]
    TMP_Text scoreValueText;
    
    [SerializeField]
    TMP_Text ballsRemainingValueText;

    private int ballsRemaining;

    public WaitForSeconds HidePanelDelay { get; private set; }

    private int scorePerBasket;

    private void OnEnable()
    {
        ThrowController.UpdateUIListener += OnUpdateUIListener;
        BasketCheckher.BasketListener += OnBasketListener;
    }

    private void OnDisable()
    {
        ThrowController.UpdateUIListener -= OnUpdateUIListener;
        BasketCheckher.BasketListener -= OnBasketListener;
    }

    private void OnBasketListener()
    {
        // increase score
        GameManager.Instance.Score += scorePerBasket;

        //update text
        scoreValueText.text = GameManager.Instance.Score.ToString();
    }

    private void OnUpdateUIListener()
    {
        --ballsRemaining;
        ballsRemainingValueText.text = ballsRemaining.ToString();

        if (ballsRemaining == 0)
            StartCoroutine(HideGameplayPanel());
    }

    private void Start()
    {
        HidePanelDelay = new WaitForSeconds(3);

        // Get level values
        scorePerBasket = LevelController.Instance.GetScorPerBasketValue();
        ballsRemaining = LevelController.Instance.BallsRemaining;

        // Initialize texts
        ballsRemainingValueText.text = ballsRemaining.ToString();
        scoreValueText.text = GameManager.Instance.Score.ToString();
    }

    private IEnumerator HideGameplayPanel()
    {
        yield return HidePanelDelay;

        gameplayPanel.SetActive(false);
    }
}
