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
        Debug.Log("OnEnable");
        LevelController.EndGameListener += OnEndGameListener;
    }

    private void OnDisable()
    {
        Debug.Log("OnDisaable");
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

    public void ContinueButton()
    {
        //Increase the level number by one.
        int currentLevel = PlayerPrefs.GetInt("LevelNumber");
        ++currentLevel;
        PlayerPrefs.SetInt("LevelNumber", currentLevel);

        //Reload the level to create it according to the new level number
        LoadScene();
    }

    private IEnumerator ShowEndGamePanel()
    {
        yield return showPanelDelay;

        scoreText.text = GameManager.Instance.Score.ToString();

        endGamePanel.SetActive(true);

    }

    private void LoadScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
