using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameUIController : MonoBehaviour
{
    public delegate void EndGameAction();

    [SerializeField]
    GameObject endGamePanel;

    [SerializeField]
    TMP_Text scoreText;

    private WaitForSeconds showPanelDelay;

    private void OnEnable()
    {
        LevelController.EndGameListener += OnEndGameListener;
    }

    private void OnDisable()
    {
        LevelController.EndGameListener -= OnEndGameListener;
    }

    private void OnEndGameListener()
    {
        StartCoroutine(ShowEndGamePanel());
    }

    private void Start()
    {
        showPanelDelay = new WaitForSeconds(3);
    }

    private IEnumerator ShowEndGamePanel()
    {
        yield return showPanelDelay;

        scoreText.text = GameManager.Instance.Score.ToString();

        endGamePanel.SetActive(true);
    }

    public void ContinueButton()
    {
        IncreaseLevelNumber();

        //Reload the level to create it according to the new level number
        LoadScene();
    }

    private static void IncreaseLevelNumber()
    {
        //Increase the level number by one.
        int currentLevel = LevelController.Instance.GetLevelNumber();

        ++currentLevel;
        PlayerPrefs.SetInt("LevelNumber", currentLevel);
    }

    private void LoadScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

}
