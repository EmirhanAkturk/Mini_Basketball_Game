using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameUIController : MonoBehaviour
{
    public delegate void EndGameAction();

    //[SerializeField]
    //GameObject endGamePanel;

    [SerializeField]
    TMP_Text scoreText;

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
        Debug.Log("EVENT SENDEDD!!");
        gameObject.SetActive(true);
        scoreText.text = GameManager.Instance.Score.ToString();
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

    private void LoadScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
